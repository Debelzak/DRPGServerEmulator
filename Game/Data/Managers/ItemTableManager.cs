using System.Xml.Linq;
using DRPGServer.Game.Data.Models;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class ItemTableManager
    {
        private static readonly string path = "Resources/Item/ItemTable.xml";

        public static List<ItemTableEntry> ItemTable { get; private set; } = new();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"ItemTable file [{path}] not found.");

            var doc = XDocument.Load(path);
            var itemTable = doc.Element("ItemTable")?.Elements("Item");

            if (itemTable is null)
                throw new Exception("Missing <ItemTable> root.");

            foreach (var digimon in itemTable)
            {
                var itemId = uint.Parse(digimon.Attribute("ID")?.Value ?? "0");
                var itemName = digimon.Attribute("Name")?.Value ?? "0";
                if (!ItemTable.Any(d => d.ItemID == itemId))
                {
                    var item = new ItemTableEntry()
                    {
                        ItemID = itemId,
                        Name = itemName,
                    };

                    ItemTable.Add(item);
                }
                else
                {
                    throw new Exception($"[ItemTable] Duplicated item {itemId}");
                }
            }
            
            Logger.Info($"[RESOURCES] Loaded {ItemTable.Count} Item data.");
        }

        public static void Reload()
        {
            ItemTable.Clear();
            Load();
        }
    }
}