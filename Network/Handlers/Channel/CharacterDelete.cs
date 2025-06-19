using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterDelete : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_DELETE;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            // Send response
            var data = new CharacterDeletePacket();
            client.Send(data);

            // Send chracter list again
            var data2 = new CharacterListPacket();
            client.Send(data2);
        }
    }
}