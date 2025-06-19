using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x0e_Packet() : OutPacket((ushort)PACKET_ID._0x000e_PACKET)
    {
        protected override void Serialize()
        {
            Write([
                0x9b, 0x8a, 0x2c, 0xe3, 0x19, 0xdb, 0x14, 0xda,
                0x66, 0x9a, 0x27, 0xf0, 0x74, 0xbf, 0x0e, 0x7c,
            ]);
        }
    }
}