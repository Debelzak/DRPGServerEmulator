using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Global;
using DRPGServer.Network.Packets.Global;

namespace DRPGServer.Network.Handlers.Global
{
    class _0xde_Handler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.GLOBAL_CHARACTER_JOIN_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.GLOBAL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            //var _0xe1_PACKET = new _0xe1_Packet();
            //client.Send(_0xe1_PACKET);
        }
    }
}