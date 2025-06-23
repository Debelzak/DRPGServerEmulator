using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map.Character
{
    public class MoveCharacterPacket() : OutPacket((ushort)PACKET_ID.MAP_MOVE_CONFIRM)
    {
        protected override void Serialize()
        {
            
        }
    }
}