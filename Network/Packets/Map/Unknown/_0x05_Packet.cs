using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x05_Packet() : OutPacket((ushort)PACKET_ID._0x05_PACKET)
    {
        protected override void Serialize()
        {
            WriteBytes(new byte[164]);   // Full of zeroes
        }
    }
}