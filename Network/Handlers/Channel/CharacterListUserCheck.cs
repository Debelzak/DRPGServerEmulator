using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterListUserCheck : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_USER_CHECK;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var data = new CharacterListUserCheckPacket();
            client.Send(data);
        }
    }
}