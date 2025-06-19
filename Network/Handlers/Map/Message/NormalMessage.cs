using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map.Message;

namespace DRPGServer.Network.Handlers.Map.Message
{
    class NormalMessageHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_CHAT_MESSAGE_NORMAL;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            string Nickname = packet.ReadString(21);
            string Message = packet.ReadString(44);
            var data = new NormalMessagePacket()
            {
                Nickname = Nickname,
                Message = Message
            };

            client.Send(data);
        }
    }
}