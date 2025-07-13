using DRPGServer.Common;
using DRPGServer.Game.Data.Models;

namespace DRPGServer.Game.Entities
{
    public class Character
    {
        public Serial Serial { get; private set; } = new();
        public uint UID;
        public byte TamerID;
        public string Name = string.Empty;
        public ushort Level;
        public byte[] EquippedItems = new byte[44];
        public Inventory Inventory { get; set; } = new();
        public Digimon MainDigimon { get; set; } = Digimon.Empty;
        public double Bits { get; private set; }
        public int TotalBattles;
        public int TotalWins;
        public byte MapID;
        public short PositionX { get; set; }
        public short PositionY { get; set; }

        public static Character Empty { get; } = new();

        public void AddBits(double incomingValue)
        {
            Bits += incomingValue;
            if (Bits < 0) Bits = 0;
        }
    }
}