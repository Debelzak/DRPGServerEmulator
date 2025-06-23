using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map.Character
{
    class RefreshCharacterInfoPacket() : OutPacket((ushort)PACKET_ID.MAP_REFRESH_CHARACTER_INFO)
    {
        public byte[] CharacterKey { get; set; } = new byte[16];
        public string Nickname { get; set; } = string.Empty;
        public byte TamerID { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public ushort DigimonID { get; set; }
        public byte[] DigimonKey { get; set; } = new byte[16];
        public string DigimonNickname { get; set; } = string.Empty;
        protected override void Serialize()
        {
            WriteBytes(CharacterKey); // byte key
            WriteString(Nickname, 21);
            WriteByte(TamerID); // TamerID
            WriteUShort((ushort)1); // ?
            WriteUShort((ushort)1); // ?
            WriteUShort((ushort)1); // ?
            WriteShort(PositionX); // PositionX
            WriteShort(PositionY); // PositionY
            WriteUShort((ushort)0x003f);
            WriteUShort((ushort)0x0058);
            WriteInt(0);
            WriteBytes(DigimonKey); // Another byte key
            WriteUShort(DigimonID); // DigimonID
            WriteString(DigimonNickname, 21); // DigimonName
            WriteByte((byte)1);
            WriteInt(1);
            WriteBytes(new byte[100]);
        }
    }
}