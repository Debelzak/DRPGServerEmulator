using DRPGServer.Network.Enum.Login;

namespace DRPGServer.Network.Packets
{
    class FirstHandshakePacket() : OutPacket((ushort)PACKET_ID.LOGIN_FIRST_HANDSHAKE)
    {
        public string Username { get; set; } = string.Empty;

        protected override void Serialize()
        {
            WriteInt(3);
            WriteInt(0x68);
            WriteString(Username, 20);
        }
    }
}