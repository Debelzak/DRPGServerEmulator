using DRPGServer.Common;

namespace DRPGServer.Game.Entities
{
    public class Character
    {
        public Serial Serial { get; private set; }
        public uint UID = 0;
        public byte TamerID;
        public string Nickname = string.Empty;
        public ushort Level;
        public byte[] EquippedItems = new byte[44];
        public Digimon MainDigimon { get; private set; }
        public double Bits { get; private set; }
        public int TotalBattles;
        public int TotalWins;
        public byte LocationID;
        public short PositionX { get; set; }
        public short PositionY { get; set; }

        public static Character Empty { get; } = new(Digimon.Empty);

        public Character(Digimon mainDigimon)
        {
            MainDigimon = mainDigimon;
            MainDigimon.Serial = new(Utils.GenerateRandomSessionId(true));
            Serial = new(Utils.GenerateRandomSessionId(false));
        }

        public void AddBits(double incomingValue)
        {
            Bits += incomingValue;
            if (Bits < 0) Bits = 0;
        }
    }
}