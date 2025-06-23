using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class _0x0e_Packet() : OutPacket((ushort)PACKET_ID._0x0e_PACKET)
    {
        protected override void Serialize()
        {
            WriteBytes(Utils.GenerateRandomSessionId());
        }
    }
}