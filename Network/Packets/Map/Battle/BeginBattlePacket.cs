using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map.Battle
{
    public class BeginBattlePacket() : OutPacket((ushort)PACKET_ID.MAP_BATTLE_BEGIN)
    {
        protected override void Serialize()
        {
            //Empty packet, its just an ack from server that the battle can begin.
        }
    }
}