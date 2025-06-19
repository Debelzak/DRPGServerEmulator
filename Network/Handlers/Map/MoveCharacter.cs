using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class MoveCharacterHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_MOVE_CHARACTER_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            int unknown_1 = packet.ReadShort();
            short positionX = packet.ReadShort();
            short positionY = packet.ReadShort();
            var data = new MoveCharacterPacket();
            client.Send(data);
        }
    }
}