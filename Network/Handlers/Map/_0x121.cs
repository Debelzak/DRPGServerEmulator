using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class _0x121_Handler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID._0x121_PACKET;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var data = new _0x121_Packet();
            client.Send(data);
        }
    }
}