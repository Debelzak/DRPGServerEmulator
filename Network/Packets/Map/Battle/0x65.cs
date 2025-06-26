using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x65_Packet() : OutPacket((ushort)PACKET_ID.MAP_BATTLE_65)
    {
        protected override void Serialize()
        {
            WriteBytes(new byte[16]);   // Digimon Serial.
        }
    }
}