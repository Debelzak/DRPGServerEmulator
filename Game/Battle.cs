using DRGPServer.Managers;
using DRPGServer.Common;
using DRPGServer.Game.Entities;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game
{
    public class Battle : IDisposable
    {
        const int UPDATE_TICK_INTERVAL_MS = 200;

        public Serial Serial { get; private set; } = new();

        public List<Player> Participants { get; private set; } = [];
        public List<Digimon> TeamA { get; } = [];
        public List<Digimon> TeamB { get; } = [];

        public bool IsDisposed { get; private set; }

        public bool IsIdle { get; private set; }
        private bool IsPvP { get; set; }
        public bool IsReady { get; set; }

        private CancellationTokenSource _cts = new();
        private WildDigimon? wildDigimon { get; set; }
        private readonly Random dice = new();

        // Action queue
        private readonly Dictionary<Serial, Action?> enqueuedActions = [];
        private readonly Queue<Action> actionQueue = new();

        public Battle(Player participant, WildDigimon enemy)
        {
            // TeamA
            Participants.Add(participant);
            TeamA.Add(participant.Character.MainDigimon);

            // TeamB
            for (int i = 0; i < 5; i++)
            {
                if (enemy.Digimons.Count < (i + 1)) break;

                // First digimon is mandatory
                if (i == 0)
                {
                    TeamB.Add(enemy.Digimons[i]);
                }
                else
                {
                    if (dice.Next(0, 101) <= enemy.Spawn.DigimonPool[i].AppearanceRate)
                        TeamB.Add(enemy.Digimons[i]);
                }
            }

            wildDigimon = enemy;

            Logger.Debug($"[BATTLE] New PVE battle instance created. ID [{Serial.ToString()}].");
        }

        public Battle(Player participant_a, Player participant_b)
        {
            IsPvP = true;

            // TeamA
            Participants.Add(participant_a);
            TeamA.AddRange(participant_a.Character.MainDigimon);

            // TeamB
            Participants.Add(participant_b);
            TeamB.AddRange(participant_b.Character.MainDigimon);

            Logger.Debug($"[BATTLE] New PVP battle instance created. ID [{Serial.ToString()}].");
        }

        public void Start()
        {
            foreach (var participant in Participants)
            {
                if (participant.Client == null) throw new Exception("[BATTLE] Trying to start battle with a disonnected player.");

                participant.Battle = this;
                participant.CanMove = false;
            }

            // Prepare all digimons
            var allDigimons = TeamA.Concat(TeamB);
            foreach (var digimon in allDigimons)
            {
                enqueuedActions.Add(digimon.Serial, null);
                digimon.CurrentActionGauge = 0;
            }

            var startBattlePacket = new StartBattlePacket(this);
            Broadcast(startBattlePacket);

            _cts = new CancellationTokenSource();
            Task.Run(async () => 
            {
                await Task.Delay(700); // little delay for the animation of entering battle.
                while (!_cts.Token.IsCancellationRequested)
                {
                    Update();
                    await Task.Delay(UPDATE_TICK_INTERVAL_MS);
                }
            }, _cts.Token);
        }

        public void StopLoop()
        {
            _cts?.Cancel();
        }

        public void Ready()
        {
            IsReady = true;
        }

        private void Update()
        {
            // End battle if TeamA or TeamB has no alive members
            // Win
            if (TeamA.All(d => d.IsKnockedOut))
                EndBattle(false);

            if (TeamB.All(d => d.IsKnockedOut))
                EndBattle(true);

            if (Participants.Count < 1 && !IsDisposed)
                Dispose();

            if (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                IsReady = false;
                IsIdle = false;
                action();
                return;
            }

            if (IsReady && actionQueue.Count <= 0)
                IsIdle = true;
                 
            // Accumulate action bar only if is idle
            if (IsIdle)
            {
                var packets = new List<ActionCooldownPacket>();
                var allDigimons = TeamA.Concat(TeamB);

                // Accumulate gauge
                foreach (var digimon in allDigimons)
                {
                    if (!IsIdle) break;
                    if (digimon.IsKnockedOut) continue;

                    var enqueuedAction = enqueuedActions[digimon.Serial];

                    // Accumulate and update one bar tick
                    digimon.CurrentActionGauge += GetSpeed(digimon);
                    digimon.CurrentActionGauge = digimon.IsActionGaugeFull ? digimon.MaxActionGauge : digimon.CurrentActionGauge;
                    var packet = new ActionCooldownPacket(digimon);
                    packets.Add(packet);

                    // Controls Wild AI
                    if (wildDigimon != null && TeamB.Contains(digimon) && enqueuedAction == null)
                    {
                        var dice = new Random().Next(TeamA.Count);
                        var target = TeamA[dice];
                        SetAutoAttackQueue(digimon, target, false);
                    }

                    // if its time to attack
                    if (digimon.IsActionGaugeFull && enqueuedAction != null)
                    {
                        actionQueue.Enqueue(enqueuedAction);
                        //ResetAutoAttackQueue(digimon);
                        break;
                    }
                }

                // Send packet to all connected clients
                foreach (var packet in packets)
                    Broadcast(packet);
            }
        }

        public void ActionRequest(string requesterSerial, string targetSerial)
        {
            if (!IsIdle) return;

            var allBattleDigimons = TeamA.Concat(TeamB);

            var requester = allBattleDigimons.FirstOrDefault(d => d.Serial.ToString() == requesterSerial);
            var target = allBattleDigimons.FirstOrDefault(d => d.Serial.ToString() == targetSerial);

            if (requester == null || target == null) return;

            // Basic attack
            if (enqueuedActions[requester.Serial] != null)
                ResetAutoAttackQueue(requester);
            
            SetAutoAttackQueue(requester, target, !requester.IsActionGaugeFull);
        }

        private void Attack(Digimon requester, Digimon target)
        {
            IsIdle = false;

            var lockActionBar = new LockActionBarPacket();
            Broadcast(lockActionBar);

            var damage = DamageCalc(requester, target);
            target.TakeDamage(damage);

            var action = new ActionResultsPacket(requester, target, this);
            Broadcast(action);

            if (target.IsKnockedOut)
            {
                ResetAutoAttackQueue(requester);
                HandleBattleRewards(requester, target);
            }

            requester.CurrentActionGauge = 0;
        }

        private void HandleBattleRewards(Digimon digimon, Digimon defeated)
        {
            // Define reward value
            long rewardExp = 0;
            wildDigimon?.ExpRewardTable.TryGetValue(defeated.Serial.ToString(), out rewardExp);

            double rewardBits = 0;
            wildDigimon?.BitRewardTable.TryGetValue(defeated.Serial.ToString(), out rewardBits);

            // Effectively add user rewards
            digimon.AddExp(rewardExp);
            digimon.Owner?.Character.AddBits(rewardBits);

            // Send visual feedback to client
            var expBitReward = new RewardBitExpPacket(digimon, rewardExp, rewardBits);
            digimon.Owner?.Client.Send(expBitReward);
        }

        private void ResetAutoAttackQueue(Digimon requester)
        {
            enqueuedActions[requester.Serial] = null;
            var actionReqPacket = new ActionRequestPacket(requester) { Action = 0 };
            Broadcast(actionReqPacket);
        }

        private void SetAutoAttackQueue(Digimon requester, Digimon target, bool visualFeedback = false)
        {
            enqueuedActions[requester.Serial] = () => Attack(requester, target);
            if (!visualFeedback) return;
            
            var actionReqPacket = new ActionRequestPacket(requester, target) { Action = 1 };
            Broadcast(actionReqPacket);
        }

        private static int DamageCalc(Digimon attacker, Digimon target)
        {
            const float BR_MULT = 0.4f;
            const float DEF_MULT = 0.22f;

            float rawDamage = (attacker.ATK * BR_MULT) - (target.DEF * DEF_MULT);
            int finalDamage = (int)Math.Round(rawDamage);

            return Math.Max(0, finalDamage);
        }

        private static int GetSpeed(Digimon digimon)
        {
            if (digimon.Owner == null) return 10;

            return digimon.Level switch
            {
                >= 30 => 101,
                >= 20 => 92,
                >= 5 => 90,
                >= 3 => 83,
                >= 2 => 70,
                _ => 38,
            };
        }

        private void Broadcast(OutPacket packet)
        {
            foreach (var participant in Participants)
            {
                if (!participant.Client.IsDisposed)
                    participant.Client.Send(packet);
                else
                    packet.Dispose();
            }
        }

        public void KillTeamA()
        {
            IsIdle = false;
            foreach (var digimon in TeamA)
            {
                digimon.TakeDamage(digimon.MaxHP);
                var action = new ActionResultsPacket(digimon, digimon, this);
                Broadcast(action);
            }
        }

        public void KillTeamB()
        {
            IsIdle = false;
            foreach (var digimon in TeamB)
            {
                digimon.TakeDamage(digimon.MaxHP);
                var action = new ActionResultsPacket(digimon, digimon, this);
                Broadcast(action);
            }
        }

        private void EndBattle(bool win = false, bool battleCancel = false)
        {
            BattleEndPacket battleResult;

            if (win) battleResult = new BattleEndPacket(1);
            else battleResult = new BattleEndPacket(2);

            Broadcast(battleResult);
            Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            StopLoop();

            foreach (var participant in Participants)
            {
                participant.Battle = null;
                participant.CanMove = true;
            }

            Participants.Clear();
            wildDigimon?.Reset();
            BattleManager.DeleteBattle(this);

            Logger.Debug($"[BATTLE] Battle id [{Serial.ToString()}] terminated.");

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}