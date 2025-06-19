using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map.Message
{
    public class NormalMessagePacket() : OutPacket((ushort)PACKET_ID.MAP_CHAT_MESSAGE_NORMAL)
    {
        public string Nickname { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        protected override void Serialize()
        {
            Write(Nickname, 21);
            Write(Message, 44);
        }
    }
}