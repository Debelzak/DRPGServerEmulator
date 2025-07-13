using DRPGServer.Game.Data.Managers;

namespace DRPGServer.Game.Data.Models
{
    public class Inventory
    {
        private Item?[] ItemInventory { get; set; } = new Item?[24];
        private Item?[] CardInventory { get; set; } = new Item?[24];

        private readonly uint maxInventoryItems = (uint)ServerConsts.Get("MAX_INVENTORY_ITEMS");

        public Item? TryAddItem(uint itemId, uint amount)
        {
            // Check for existing item.
            var item = GetItemByID(itemId);
            if (item != null)
            {
                var newAmount = item.Amount + amount;
                if (newAmount > maxInventoryItems)
                    item.Amount = maxInventoryItems;
                else
                    item.Amount = newAmount;
                
                return item;
            }

            // Add new item
            for (int i = 0; i < 24; i++)
            {
                if (ItemInventory[i] == null)
                {
                    var newItem = new Item()
                    {
                        UID = GenerateUID(),
                        ItemID = itemId,
                        Amount = (amount > maxInventoryItems) ? maxInventoryItems : amount,
                        SlotPos = (uint)i
                    };

                    ItemInventory[i] = newItem;
                    return ItemInventory[i];
                }
            }

            return null;
        }

        public bool TryRemoveItem(uint itemId, uint amount)
        {
            var item = GetItemByID(itemId);
            if (item == null)
                return false;

            if (item.Amount < amount)
                return false;

            item.Amount -= amount;

            if (item.Amount == 0)
            {
                ItemInventory[item.SlotPos] = null;
            }

            return true;
        }

        public bool HasSpaceForItem(int slotNum = 1)
        {
            return ItemInventory.Count(slot => slot == null) >= slotNum;
        }

        public bool HasSpaceForCard(int slotNum = 1)
        {
            return CardInventory.Count(slot => slot == null) >= slotNum;
        }

        private uint GenerateUID()
        {
            return (uint)Random.Shared.Next(10000, 99999);
        }

        public Item? GetSlot(int slotIndex)
        {
            return slotIndex >= 0 && slotIndex < 24 ? ItemInventory[slotIndex] : null;
        }

        public Item? GetItemByID(uint itemId)
        {
            return ItemInventory.FirstOrDefault(d => d != null && d.ItemID == itemId);
        }

        public Item? GetItemByUID(uint itemUID)
        {
            return ItemInventory.FirstOrDefault(d => d != null && d.UID == itemUID);
        }
    }
}