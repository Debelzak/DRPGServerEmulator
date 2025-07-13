using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterCreatePacket() : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_CREATE)
    {
        public int ErrorCode { get; set; } = 0;
        public byte CharacterModel { get; set; } = 0;
        public string Nickname { get; set; } = string.Empty;
        public ushort DigimonId { get; set; } = 0;
        public string DigimonNickname { get; set; } = string.Empty;

        protected override void Serialize()
        {
            WriteInt((ErrorCode == 0) ? 2 : 3);               // Unknown (Client 1, Success from server 2, error 3)
            WriteInt(ErrorCode);               // Error code? (If above=2, this=0); 102 = Tamer Already exists
            WriteShort((short)0x4f01);   // Unknown
            WriteShort((short)0x003d);   // Random (3B, 3C, 3D, 3E)
            WriteShort((short)0x0038);   // Random (37, 38, 39, 3A)
            WriteByte(CharacterModel);         // Character Model
            WriteString(Nickname, 21);  // Character Name
            WriteUShort(DigimonId);      // Digimon ID
            WriteString(DigimonNickname, 26);   // Digimon Name
        }
    }
}