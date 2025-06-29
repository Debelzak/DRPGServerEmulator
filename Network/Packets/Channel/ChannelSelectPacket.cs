using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class ChannelSelectPacket() : OutPacket((ushort)PACKET_ID.CHANNEL_SELECT)
    {
        protected override void Serialize()
        {
            WriteInt(2);               // 2 = Connect ; 3 = Error ; 
            WriteInt(0);
            WriteInt(0); // Fourth rotational digimon choice: 
                         // 0 = Chibimon
                         // 1 = Yarmon
                         // 2 = Hopmon
                         // 3 = Dorimon
            WriteString("MoveInteractive_Digimon_035", 40);
        }
    }
}