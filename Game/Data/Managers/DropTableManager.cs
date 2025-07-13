using System.Xml.Linq;
using DRPGServer.Game.Data.Models;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class DropTableManager
    {
        private static readonly string path = "Resources/Item/ItemDrop.xml";

        public static Dictionary<uint, List<DropTableEntry>> DropTable { get; private set; } = [];

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"DropTable file [{path}] not found.");

            var doc = XDocument.Load(path);
            var dropTable = doc.Element("DropTable")?.Elements("DropGroup");

            if (dropTable is null)
                throw new Exception("Missing <DropTable> root.");

            foreach (var dropGroup in dropTable)
            {
                var groupId = uint.Parse(dropGroup.Attribute("ID")?.Value ?? "0");
                var items = dropGroup.Elements("Item");
                List<DropTableEntry> toAddArray = [];
                foreach (var item in items)
                {
                    var itemId = uint.Parse(item.Attribute("ID")?.Value ?? "0");
                    var dropRate = byte.Parse(item.Attribute("DropRate")?.Value ?? "0");
                    if (ItemTableManager.ItemTable.Any(i => i.ItemID == itemId))
                    {
                        if (dropRate < 0 || dropRate > 100)
                            throw new Exception($"DropGroup ID [{groupId}], ItemID [{itemId}] has an invalid DropRate [{dropRate}]. DropRate must be between 0 and 100.");

                        var toAdd = new DropTableEntry()
                        {
                            ItemID = itemId,
                            DropRate = dropRate,
                        };

                        toAddArray.Add(toAdd);
                    }
                    else
                    {
                        throw new Exception($"DropGroup ID [{groupId}] has an unknown item [{itemId}].");
                    }
                }

                DropTable.Add(groupId, toAddArray);
            }
            
            Logger.Info($"[RESOURCES] Loaded {DropTable.Count} DropGroup data.");
        }

        public static void Reload()
        {
            DropTable.Clear();
            Load();
        }
    }
}