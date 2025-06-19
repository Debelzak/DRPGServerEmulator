using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class ChannelSelectPacket() : OutPacket((ushort)PACKET_ID.CHANNEL_SELECT)
    {
        protected override void Serialize()
        {
            Write(2);               // 2 = Connect ; 3 = Error ; 
            Write(0);
            Write(3);
            Write("MoveInteractive_Digimon_035", 40);
        }
    }
}