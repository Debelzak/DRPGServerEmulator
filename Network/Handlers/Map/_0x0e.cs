using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network.Handlers.Map
{
    class _0x0e_Handler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID._0x0e_PACKET;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {

        }
    }
}