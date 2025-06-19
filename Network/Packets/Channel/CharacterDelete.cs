using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterDeletePacket() : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_DELETE)
    {
        protected override void Serialize()
        {
            Write(2);  
            Write(0);
            Write(20000);
            Write(Utils.MD5(string.Empty), 40);
        }
    }
}