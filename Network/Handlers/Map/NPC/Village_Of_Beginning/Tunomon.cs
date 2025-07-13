using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map.NPC
{
    class Tunomon
    {
        public static void Handle(Client client, uint choiceId)
        {
            var inventory = client.Player?.Character.Inventory;
            if (inventory == null) return;

            if (!inventory.TryRemoveItem(41, 5)) return;
            
            var item = inventory.TryAddItem(22008, 1);
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