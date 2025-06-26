using DRGPServer.Managers;
using DRPGServer.Common;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game
{
    public class Zone
    {
        const int AUTO_RECOVERY_TICK = 5_000;
        const int WILD_DIGIMON_MOVE_TICK_INTERVAL = 2_000;

        public byte MapID { get; }
        private List<Player> players = [];
        private List<Portal> portals = [];
        public IReadOnlyList<Portal> Portals => portals;
        private List<DigimonSpawn> spawns = [];
        private Dictionary<string, WildDigimon> wildDigimons = [];

        private double wildDigimonTickTimer;
        private double autoHealTickTimer;

        private static readonly Random dice = new();

        public Zone(byte mapId)
        {
            MapID = mapId;
        }

        public void Start()
        {
            InitSpawns();
            InitPortals();
        }

        public void Update(double deltaTime)
        {
            // Wild digimon movement tick
            wildDigimonTickTimer += deltaTime;
            if (wildDigimonTickTimer >= WILD_DIGIMON_MOVE_TICK_INTERVAL)
            {
                wildDigimonTickTimer = 0;
                OnWildDigimonUpdateTick();
            }

            // Player auto recovery per time
            autoHealTickTimer += deltaTime;
            if (autoHealTickTimer >= AUTO_RECOVERY_TICK)
            {
                autoHealTickTimer = 0;
                OnAutoRecovery();
            }
        }

        private void InitSpawns()
        {
            foreach (var spawn in SpawnDataManager.Spawns)
            {
                if (spawn.MapID == MapID) AddSpawn(spawn);
            }

            foreach (var spawn in spawns)
            {
                for (int i = 0; i < spawn.MaxCount; i++)
                {
                    var wildDigimon = new WildDigimon(spawn);
                    AddWildDigimon(wildDigimon);
                }
            }
        }

        public void ReloadSpawns()
        {
            foreach (var digimon in wildDigimons.Values)
            {
                digimon.IsDead = true;
                var packet = new WildDigimonUpdatePacket(digimon);
                Broadcast(packet);
            }

            spawns.Clear();
            SpawnDataManager.Reload();
            wildDigimons.Clear();
            InitSpawns();
            OnWildDigimonUpdateTick();
        }

        private void InitPortals()
        {
            foreach (var portal in PortalDataManager.Portals)
            {
                if (portal.MapID == MapID) AddPortal(portal);
            }
        }

        private void OnAutoRecovery()
        {
            foreach (var player in players)
            {
                if (player.Battle != null) continue;

                player.Character.MainDigimon.Heal(5, 5, 0);

                var recoveryStatus = new RefreshDigimonStatusPacket(player.Character.MainDigimon);
                player.Client.Send(recoveryStatus);
            }
        }

        private void OnWildDigimonUpdateTick()
        {
            int moveChance = 50; // Each tick each digimon has a moveChance% chance of moving.
            List<WildDigimon> toRespawn = [];

            foreach (var wildDigimon in wildDigimons.Values)
            {
                // Respawn
                if (wildDigimon.IsDead)
                {
                    wildDigimon.RespawnCooldown -= WILD_DIGIMON_MOVE_TICK_INTERVAL;
                    if (wildDigimon.RespawnCooldown <= 0)
                        toRespawn.Add(wildDigimon);

                    continue;
                }

                // Movement
                var spawn = spawns.FirstOrDefault(d => d.DigimonID == wildDigimon.Leader.DigimonID);
                if (spawn == null) continue;

                if (dice.Next(0, 100) <= moveChance)
                {
                    var stepsX = dice.Next(-3, 4);
                    var stepsY = dice.Next(-3, 4);
                    short newX = (short)Math.Clamp(wildDigimon.PositionX + stepsX, spawn.PosXMin, spawn.PosXMax);
                    short newY = (short)Math.Clamp(wildDigimon.PositionY + stepsY, spawn.PosYMin, spawn.PosYMax);

                    if (newX != wildDigimon.PositionX || newY != wildDigimon.PositionY)
                    {
                        wildDigimon.MovePosition(newX, newY);
                    }
                }

                var packet = new WildDigimonUpdatePacket(wildDigimon);
                Broadcast(packet);
            }

            // Respawn
            foreach (var deadDigimon in toRespawn)
            {
                var spawn = deadDigimon.Spawn;
                wildDigimons.Remove(deadDigimon.Serial.ToString());

                var spawnedDigimon = new WildDigimon(spawn);
                wildDigimons.Add(spawnedDigimon.Serial.ToString(), spawnedDigimon);

                var packet = new WildDigimonUpdatePacket(spawnedDigimon);
                Broadcast(packet);
            }
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            Logger.Debug($"Player [{player.Character.Nickname}] entered zone [{(MAP_ID)MapID}]");
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            Logger.Debug($"Player [{player.Character.Nickname}] exited zone [{(MAP_ID)MapID}]");
        }

        public void AddPortal(Portal portal)
        {
            portals.Add(portal);
        }

        public void AddSpawn(DigimonSpawn spawn)
        {
            spawns.Add(spawn);
        }

        public void AddWildDigimon(WildDigimon wildDigimon)
        {
            wildDigimons.Add(wildDigimon.Serial.ToString(), wildDigimon);
        }

        public void RemoveWildDigimon(WildDigimon wildDigimon)
        {
            wildDigimons.Remove(wildDigimon.Serial.ToString());
        }

        public void Broadcast(OutPacket packet)
        {
            foreach (var player in players)
            {
                player.Client.Send(packet);
            }
        }

        public void BroadcastExceptPlayer(OutPacket packet, Player sender)
        {
            foreach (var player in players)
            {
                if (player == sender) continue;

                player.Client.Send(packet);
            }
        }

        public Battle? RequestBattle(Player participant, byte[] enemyRequestSerial)
        {
            Serial serial = new(enemyRequestSerial);
            if (!wildDigimons.TryGetValue(serial.ToString(), out var enemy)) return null;
            if (enemy.IsBusy || enemy.IsDead) return null;

            var battle = BattleManager.CreateBattle(participant, enemy);
            return battle;
        }
    }
}