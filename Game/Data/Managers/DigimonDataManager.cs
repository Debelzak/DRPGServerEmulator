using System.Xml.Linq;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class DigimonDataManager
    {
        private static readonly string path = "Resources/Digimon/DigimonData.xml";
        public static Dictionary<int, Digimon> DigimonTable { get; private set; } = new();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"DigimonData file [{path}] not found.");

            var doc = XDocument.Load(path);
            var digimons = doc.Element("DigimonData")?.Elements("Digimon");

            if (digimons is null)
                throw new Exception("Missing <DigimonData> root.");

            DigimonTable.Clear();

            foreach (var digimon in digimons)
            {
                var digimonId = ushort.Parse(digimon.Attribute("ID")?.Value ?? "0");
                if (digimonId == 0) continue;

                var toAdd = new Digimon(digimonId)
                {
                    Name = digimon.Attribute("Name")?.Value ?? string.Empty,
                };

                DigimonTable.Add(digimonId, toAdd);
            }

            Logger.Info($"[RESOURCES] Loaded {DigimonTable.Count} Digimon data.");
        }

        public static void Reload()
        {
            DigimonTable.Clear();
            Load();
        }
    }
}