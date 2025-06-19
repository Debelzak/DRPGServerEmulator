using DRPGServer.Network.Enum.Login;

namespace DRPGServer.Network.Packets.Login
{
    class FirstHandshakePacket() : OutPacket((ushort)PACKET_ID.LOGIN_FIRST_HANDSHAKE)
    {
        public string Username { get; set; } = string.Empty;

        protected override void Serialize()
        {
            Write(3);
            Write(0x68);
            Write(Username, 20);
        }
    }
}