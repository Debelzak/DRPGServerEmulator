using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterListUserCheckPacket() : OutPacket((ushort)PACKET_ID.CHANNEL_USER_CHECK)
    {
        protected override void Serialize()
        {
            WriteString("debelzak26", 22);
        }
    }
}