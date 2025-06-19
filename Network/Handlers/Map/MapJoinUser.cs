using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class TestPacket(byte[] buf) : OutPacket((ushort)PACKET_ID.MAP_INVENTORY_DATA)
    {
        protected override void Serialize()
        {
            Write(buf);
        }
    }

    class MapJoinUserHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_JOIN_USER_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var userData = new MapJoinUserPacket();
            client.Send(userData);

            var inventoryData = new InventoryDataPacket();
            client.Send(inventoryData);
        }
    }
}