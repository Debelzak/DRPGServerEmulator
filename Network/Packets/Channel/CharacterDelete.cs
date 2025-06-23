using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterDeletePacket() : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_DELETE)
    {
        public uint CharacterUID { get; set; }
        public string PasswordMD5 { get; set; } = string.Empty;
        protected override void Serialize()
        {
            WriteInt(2);
            WriteInt(0);
            WriteUInt(CharacterUID);
            WriteString(Utils.MD5(PasswordMD5), 40);
        }
    }
}