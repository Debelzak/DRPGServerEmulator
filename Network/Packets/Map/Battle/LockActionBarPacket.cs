using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class LockActionBarPacket() : OutPacket((ushort)PACKET_ID.MAP_BATTLE_LOCK_ACTIONBAR)
    {
        protected override void Serialize()
        {
            //Empty packet
        }
    }
}