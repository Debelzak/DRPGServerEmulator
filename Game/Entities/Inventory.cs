namespace DRPGServer.Game.Data.Models
{
    public class Inventory
    {
        private Item[] CardSlots { get; set; } = new Item[24];
        private Item[] ItemSlots { get; set; } = new Item[24];

        public Item? TryAddItem(uint itemId)
        {
            for (int i = 0; i < 24; i++)
            {
                if (ItemSlots[i] == null)
                {
                    var item = new Item()
                    {
                        UID = GenerateUID(),
                        ItemID = itemId,
                        InventorySlot = (uint)i
                    };

                    ItemSlots[i] = item;
                    return ItemSlots[i];
                }
            }

            return null;
        }

        public bool HasSpaceForItem()
        {
            return ItemSlots.Any(slot => slot == null);
        }

        public bool HasSpaceForCard()
        {
            return CardSlots.Any(slot => slot == null);
        }

        private uint GenerateUID()
        {
            return (uint)Random.Shared.Next(10000, 99999);
        }

        public Item? GetItemInSlot(int slotIndex)
        {
            return slotIndex >= 0 && slotIndex < 24 ? ItemSlots[slotIndex] : null;
        }
    }
}