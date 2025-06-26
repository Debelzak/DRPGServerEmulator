using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x6a_Packet() : OutPacket((ushort)PACKET_ID.MAP_BATTLE_6a)
    {
        protected override void Serialize()
        {
            WriteBytes(new byte[16]);   // Character serial.
        }
    }
}