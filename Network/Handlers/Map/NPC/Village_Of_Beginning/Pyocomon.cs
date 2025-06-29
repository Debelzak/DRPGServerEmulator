using DRPGServer.Game.Data.Models;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map.NPC
{
    class Pyocomon
    {
        public static void Handle(Client client, uint choiceId)
        {
            var inventory = client.Player?.Character.Inventory;
            if (inventory == null) return;

            var item = inventory.TryAddItem(22007);
            if (item != null)
            {
                var packet = new NPCChoicePacket(choiceId);
                client.Send(packet);
                
                var itemReceive = new ItemReceivePacket(item);
                client.Send(itemReceive);
            }
        }
    }
}