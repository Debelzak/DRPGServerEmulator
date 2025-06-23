using DRPGServer.Network.Enum.Global;

namespace DRPGServer.Network.Packets.Global
{
    class _0xe1_Packet() : OutPacket((ushort)PACKET_ID._0xe1_PACKET)
    {
        protected override void Serialize()
        {
            WriteBytes(new byte[1341]);
            WriteByte((byte)1);
        }
    }
}