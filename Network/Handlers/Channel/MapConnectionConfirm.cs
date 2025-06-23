using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class MapConnectionConfirm : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_MAP_CONNECTION_CONFIRM;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            string username = packet.ReadString(21);
            string characterNickname = packet.ReadString(21);
        }
    }
}