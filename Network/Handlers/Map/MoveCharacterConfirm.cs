using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network.Handlers.Map
{
    class MoveChracterConfirmHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_MOVE_CONFIRM;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {

        }
    }
}