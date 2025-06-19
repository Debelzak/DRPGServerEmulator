using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterCreatePacket() : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_CREATE)
    {
        public byte CharacterModel { get; set; } = 0;
        public string Nickname { get; set; } = string.Empty;
        public ushort DigimonId { get; set; } = 0;
        public string DigimonNickname { get; set; } = string.Empty;

        protected override void Serialize()
        {
            Write(2);               // Unknown (Client 1, Success from server 2, error 3)
            Write(0);               // Error code? (If above=2, this=0); 102 = Tamer Already exists
            Write((short)0x4f01);   // Unknown
            Write((short)0x003d);   // Random (3B, 3C, 3D, 3E)
            Write((short)0x0038);   // Random (37, 38, 39, 3A)
            Write(CharacterModel);         // Character Model
            Write(Nickname, 21);  // Character Name
            Write(DigimonId);      // Digimon ID
            Write(DigimonNickname, 26);   // Digimon Name
        }
    }
}