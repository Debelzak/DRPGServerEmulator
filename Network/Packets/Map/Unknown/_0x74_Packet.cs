
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x74_Packet() : OutPacket((ushort)PACKET_ID._0x74_PACKET)
    {
        protected override void Serialize()
        {
            WriteBytes(new byte[60]);
        }
    }
}
