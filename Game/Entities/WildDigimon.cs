using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DRPGServer.Common;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Game.Enum;

namespace DRPGServer.Game.Entities
{
    public class WildDigimon
    {
        public ushort DisplayDigimonID { get; private set; }
        public Serial Serial { get; private set; } = new();
        public byte MapID { get; set; }
        public short PositionX { get; private set; }
        public short PositionY { get; private set; }
        public byte FacingDirection { get; private set; }

        public List<Digimon> Digimons { get; set; } = [];
        public Dictionary<string, long> ExpRewardTable { get; private set; } = [];
        public Dictionary<string, double> BitRewardTable { get; private set; } = [];
        public DigimonSpawn Spawn { get; private set; }

        public bool IsDead { get; set; }
        public bool IsBusy { get; set; }

        public double RespawnCooldown { get; set; }

        public WildDigimon(DigimonSpawn spawn)
        {
            DisplayDigimonID = spawn.DisplayDigimonID;
            Spawn = spawn;
            var dice = new Random();

            MapID = spawn.MapID;
            RespawnCooldown = spawn.RespawnTime;

            foreach (var spawnOption in spawn.DigimonPool)
            {
                var digimon = new Digimon(spawnOption.DigimonID)
                {
                    Level = spawnOption.Level,
                    STR = spawnOption.STR,
                    AGI = spawnOption.AGI,
                    CON = spawnOption.CON,
                    INT = spawnOption.INT,
                    Name = DigimonDataManager.DigimonTable[spawnOption.DigimonID].Name
                };
                Digimons.Add(digimon);
                BitRewardTable.Add(digimon.Serial.ToString(), spawnOption.BitReward);
                ExpRewardTable.Add(digimon.Serial.ToString(), spawnOption.ExpReward);
                digimon.Heal(digimon.MaxHP, digimon.MaxVP, digimon.MaxEVP); // just in case
            }

            MovePosition((short)dice.Next(spawn.PosXMin, spawn.PosXMax), (short)dice.Next(spawn.PosYMin, spawn.PosYMax));
        }

        public void MovePosition(short newPosX, short newPosY)
        {
            short deltaX = (short)(newPosX - PositionX);
            short deltaY = (short)(newPosY - PositionY);
            FacingDirection = GetDirection(Math.Sign(deltaX), Math.Sign(deltaY));
            PositionX = newPosX;
            PositionY = newPosY;
        }

        byte GetDirection(int normX, int normY)
        {
            return (normX, normY) switch
            {
                (0, -1) => (byte)DIRECTION_ID.NORTHWEST,
                (1, -1) => (byte)DIRECTION_ID.SOUTHEAST,
                (1, 0) => (byte)DIRECTION_ID.SOUTHEAST,
                (1, 1) => (byte)DIRECTION_ID.SOUTHEAST,
                (0, 1) => (byte)DIRECTION_ID.SOUTHEAST,
                (-1, 1) => (byte)DIRECTION_ID.NORTHWEST,
                (-1, 0) => (byte)DIRECTION_ID.NORTHWEST,
                (-1, -1) => (byte)DIRECTION_ID.NORTHWEST,
                _ => (byte)DIRECTION_ID.NORTHWEST
            };
        }

        /// <summary>
        /// Fully restores HP and resets ActionBar meter.
        /// </summary>
        public void Reset()
        {
            IsBusy = false;
            foreach (var digimon in Digimons)
            {
                digimon.Heal(digimon.MaxHP, digimon.MaxVP, digimon.MaxEVP);
                digimon.CurrentActionGauge = 0;
            }
        }
    }
}