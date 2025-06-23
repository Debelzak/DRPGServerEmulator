namespace DRPGServer.Game.Entities
{
    public class Character
    {
        public uint UID = 0;
        public byte TamerID;
        public string Nickname = string.Empty;
        public ushort Level;
        public byte[] EquippedItems = new byte[44];
        public ushort DigimonID;
        public ushort DigimonLevel;
        public string DigimonNickname = string.Empty;
        public int TotalBattles;
        public int TotalWins;
        public byte LocationID;
        public short PositionX { get; set; }
        public short PositionY { get; set; }

        public static Character Empty { get; } = new();
    }
}