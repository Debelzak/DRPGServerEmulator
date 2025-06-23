using DRPGServer.Network.Enum.Global;

namespace DRPGServer.Network.Packets.Global
{
    class _0x0e_Packet() : OutPacket((ushort)PACKET_ID._0x0e_PACKET)
    {
        protected override void Serialize()
        {
            WriteBytes(Utils.GenerateRandomSessionId());
        }
    }
}