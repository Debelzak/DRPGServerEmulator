/* -- [INCOMING PACKET] [MAP_SERVER] (264 bytes) ---
0000  CC 00 91 00 08 01 00 00 0F 00 00 00 11 34 01 03   .............4..
0010  00 00 43 00 00 00 1D 00 00 00 04 00 00 00 06 00   ..C.............
0020  00 00 BC 2A 00 00 29 00 00 00 05 01 00 00 01 00   ...*..).........
0030  00 00 E7 03 00 00 E8 03 00 00 64 00 00 00 10 27   ..........d....'
0040  00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
0050  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
0060  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
0070  00 00 00 00 00 00 00 00 00 00 0F 27 00 7D 40 42   ...........'.}@B
0080  0F 00 01 01 00 01 E5 2A 00 00 00 00 00 00 00 00   .......*........
0090  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00a0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00b0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00c0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00d0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00e0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
00f0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00   ................
0100  00 00 00 00 00 00 00 00                           ........ */

using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class ItemDropGetHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_ITEM_DROP_GET;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var player = client.Player;
            if (player == null) return;

            var unk1 = packet.ReadUInt();
            var positionY = packet.ReadInt();
            var positionX = packet.ReadInt();
            var unk2 = packet.ReadInt();
            var unk3 = packet.ReadInt();
            var dropUid = packet.ReadUInt();
            var itemId = packet.ReadUInt();

            var drop = player.Zone.GetItemDrop(dropUid);
            if (drop == null) return;

            player.Zone.RemoveItemDrop(drop);

            var item = player.Character.Inventory.TryAddItem(itemId, 1);
            if (item != null)
            {
                var itemGet = new ItemDropGetPacket(drop, item);
                client.Send(itemGet);
            }

        }
    }
}