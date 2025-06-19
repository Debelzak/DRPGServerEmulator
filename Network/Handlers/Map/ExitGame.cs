using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class ExitGameHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_EXIT_GAME;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var data = new ExitGamePacket();
            client.Send(data);
        }
    }
}