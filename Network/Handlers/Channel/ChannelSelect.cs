using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class ChannelSelectHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_SELECT;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var data = new ChannelSelectPacket();
            client.Send(data);
        }
    }
}