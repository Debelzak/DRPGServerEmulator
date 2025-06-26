using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class SingleDigimonInfoHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_SINGLE_DIGIMON_INFO_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            throw new NotImplementedException();
        }
    }
}