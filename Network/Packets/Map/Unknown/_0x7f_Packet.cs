using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets
{
    class _0x7f_Packet() : OutPacket((ushort)PACKET_ID._0x7f_PACKET)
    {
        public int unknown_1 { get; set; }
        protected override void Serialize()
        {
            WriteInt(unknown_1);
            WriteBytes(new byte[2704]);
        }
    }
}