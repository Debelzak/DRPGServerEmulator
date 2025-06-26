using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DRPGServer.Common;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Game.Enum;

namespace DRPGServer.Game.Entities
{
    public class WildDigimon
    {
        public Serial Serial { get; private set; } = new();
        public byte MapID { get; set; }
        public short PositionX { get; private set; }
        public short PositionY { get; private set; }
        public byte FacingDirection { get; private set; }

        public Digimon Leader { get; set; }
        public List<Digimon> Partners { get; set; } = [];
        public DigimonSpawn Spawn { get; private set; }

        public bool IsDead { get; set; }
        public bool IsBusy { get; set; }

        public double RespawnCooldown { get; set; }

        public WildDigimon(DigimonSpawn spawn)
        {
            Spawn = spawn;
            var dice = new Random();

            MapID = spawn.MapID;
            RespawnCooldown = spawn.RespawnTime;

            Leader = new Digimon(spawn.DigimonID)
            {
                Level = spawn.Level,
                Name = DigimonDataManager.DigimonTable[spawn.DigimonID].Name
            };

            MovePosition((short)dice.Next(spawn.PosXMin, spawn.PosXMax), (short)dice.Next(spawn.PosYMin, spawn.PosYMax));

            int totalChance = 100;
            int configuredChance = spawn.PartnerPool.Sum(p => p.AppearanceRate);
            int roll = dice.Next(0, totalChance);

            if (roll < configuredChance)
            {
                int cumulative = 0;
                foreach (var partner in spawn.PartnerPool)
                {
                    cumulative += partner.AppearanceRate;
                    if (roll < cumulative)
                    {
                        for (int i = 0; i < partner.Count; i++)
                        {
                            Partners.Add(new Digimon(partner.DigimonID)
                            {
                                Level = partner.Level,
                            });

                            if (Partners.Count >= 4) break;
                        }
                        break;
                    }
                }

            }
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
            Leader.CurrentActionGauge = 0;
            Leader.Heal(Leader.MaxHP, Leader.MaxVP, Leader.MaxEVP);

            foreach (var partner in Partners)
            {
                partner.Heal(partner.MaxHP, partner.MaxVP, partner.MaxEVP);
                partner.CurrentActionGauge = 0;
            }
        }
    }
}