using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets.Login;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Login;
using DRPGServer.Game.Entities;

namespace DRPGServer.Network.Handlers.Login
{
    class FirstHandshakeHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.LOGIN_FIRST_HANDSHAKE;
        public SERVER_TYPE ServerType => SERVER_TYPE.LOGIN_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var unknown_1 = packet.ReadUInt(); //1
            var unknown_2 = packet.ReadUInt(); //0
            var username = packet.ReadString(20);

            var data = new FirstHandshakePacket
            {
                Username = username
            };

            client.Send(data);
        }
    }
}