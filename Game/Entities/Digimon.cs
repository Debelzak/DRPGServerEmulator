using System.Security.Cryptography;

namespace DRPGServer.Game.Entities
{
    public class Digimon
    {
        public uint UID { get; private set; }
        public byte[] Serial { get; private set; } = new byte[16];
        public ushort DigimonID { get; private set; }
        public string Name { get; set; } = string.Empty;
        public ushort Level { get; set; } = 1;

        public Digimon(ushort digimonId)
        {
            RandomNumberGenerator.Fill(Serial);
            DigimonID = digimonId;
        }
    }
}