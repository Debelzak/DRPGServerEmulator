using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Global;
using DRPGServer.Network.Packets.Global;

namespace DRPGServer.Network.Handlers.Global
{
    class CharacterJoinHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.GLOBAL_USER_AUTH_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.GLOBAL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            string username = packet.ReadString(21);
            string characterNickname = packet.ReadString(21);
            byte[] unknown_1 = packet.ReadBytes(22);
            int unknown_2 = packet.ReadInt();
            int unknown_3 = packet.ReadInt();
            int unknown_4 = packet.ReadInt();
            byte[] unknown_5 = packet.ReadBytes(20);
            int unknown_6 = packet.ReadInt(); // UserID? */

            var _0xe1_PACKET = new _0xe1_Packet();
            client.Send(_0xe1_PACKET);

            var _0x65_PACKET = new _0x65_Packet();
            client.Send(_0x65_PACKET);
        }
    }
}