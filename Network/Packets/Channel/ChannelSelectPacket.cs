using DRPGServer.Game.Data.Managers;
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
            WriteInt((int)ServerConsts.Get("CHARACTER_CREATE_4TH_DIGIMON"));
            WriteString("MoveInteractive_Digimon_035", 40);
        }
    }
}