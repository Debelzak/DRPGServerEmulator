using DRPGServer.Game.Entities;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class ActionResultsPacket(Digimon attacker, Digimon target, Game.Battle battle) : OutPacket((ushort)PACKET_ID.MAP_BATTLE_ACTION_RESULTS)
    {
        private bool isEnemyTurn;
        protected override void Serialize()
        {
            IReadOnlyList<Digimon> attackerTeam;
            IReadOnlyList<Digimon> targetTeam;

            if (battle.TeamB.Contains(attacker))
            {
                attackerTeam = battle.TeamB;
                targetTeam = battle.TeamA;
                isEnemyTurn = true;
            }
            else
            {
                attackerTeam = battle.TeamA;
                targetTeam = battle.TeamB;
            }

            // Team A (user)
            for (byte i = 0; i < 5; i++)
            {
                if (attackerTeam.Count > i)
                {
                    bool isAttacking = attackerTeam[i] == attacker;
                    bool isBeingAttacked = attackerTeam[i] == target;
                    WriteDigimon(attackerTeam[i], isAttacking, isBeingAttacked);
                }
                else
                {
                    WriteDigimon();
                }
            }

            // Team B (enemy)
            for (byte i = 0; i < 5; i++)
            {
                if (targetTeam.Count > i)
                {
                    bool isAttacking = targetTeam[i] == attacker;
                    bool isBeingAttacked = targetTeam[i] == target;
                    WriteDigimon(targetTeam[i], isAttacking, isBeingAttacked);
                }
                else
                {
                    WriteDigimon();
                }
            }

            WriteInt(isEnemyTurn ? 1 : 2); // 1 enemy is attacking / 2 player is attacking
            WriteBytes([0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]);
        }

        private void WriteDigimon(Digimon? digimon = null, bool isAttacking = false, bool isBeingAttacked = false)
        {
            if (digimon == null)
            {
                WriteBytes(new byte[136]);
                return;
            }

            WriteBytes(digimon.Serial.Data);
            WriteInt(digimon.CurrentHP);
            WriteInt(digimon.CurrentVP);
            WriteInt(digimon.CurrentEVP);
            WriteBytes([0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,]);
            WriteByte(isAttacking ? (byte)1 : (byte)0);
            WriteByte(isBeingAttacked ? (byte)1 : (byte)0);
            WriteBytes([
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00
            ]);
        }
    }
}