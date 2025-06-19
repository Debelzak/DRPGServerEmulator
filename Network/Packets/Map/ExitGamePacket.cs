using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class ExitGamePacket() : OutPacket((ushort)PACKET_ID.MAP_EXIT_GAME)
    {
        protected override void Serialize()
        {
            // This packet is empty by nature
        }
    }
}