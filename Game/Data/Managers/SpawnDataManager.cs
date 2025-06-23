using System.Xml.Linq;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class SpawnDataManager
    {
        private static readonly string path = "Resources/Map/DigimonSpawns.xml";
        public static List<DigimonSpawn> Spawns { get; private set; } = new();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"S file [{path}] not found.");

            var doc = XDocument.Load(path);
            var spawns = doc.Element("DigimonSpawns")?.Elements("Spawn");

            if (spawns is null)
                throw new Exception("Missing <DigimonSpawns> root.");

            foreach (var spawn in spawns)
            {
                var digimonId = ushort.Parse(spawn.Attribute("DigimonID")?.Value ?? "0");
                if (digimonId == 0) continue;

                DigimonDataManager.DigimonTable.TryGetValue(digimonId, out var digimon);
                if (digimon == null)
                {
                    throw new Exception($"[SpawnDataManager] Digimon ID [{digimonId}] was not found in DigimonData");
                }

                var add = new DigimonSpawn(digimon.DigimonID)
                {
                    MapID = byte.Parse(spawn.Attribute("MapID")?.Value ?? "0"),
                    Level = ushort.Parse(spawn.Attribute("Level")?.Value ?? "0"),
                    PosXMin = short.Parse(spawn.Attribute("PosXMin")?.Value ?? "0"),
                    PosXMax = short.Parse(spawn.Attribute("PosXMax")?.Value ?? "0"),
                    PosYMin = short.Parse(spawn.Attribute("PosYMin")?.Value ?? "0"),
                    PosYMax = short.Parse(spawn.Attribute("PosYMax")?.Value ?? "0"),
                    MaxCount = short.Parse(spawn.Attribute("MaxCount")?.Value ?? "0"),
                    RespawnTime = short.Parse(spawn.Attribute("RespawnTime")?.Value ?? "0")
                };

                var partners = spawn.Elements("SpawnPartner");
                foreach (var partner in partners)
                {
                    var partnerDigimonId = ushort.Parse(partner.Attribute("DigimonID")?.Value ?? "0");
                    DigimonDataManager.DigimonTable.TryGetValue(partnerDigimonId, out var partnerDigimon);
                    if (partnerDigimon == null)
                    {
                        throw new Exception($"[SpawnDataManager] Partner Digimon ID [{partnerDigimonId}] was not found in DigimonData");
                    }
                    
                    var partnerLevel = ushort.Parse(partner.Attribute("Level")?.Value ?? "0");
                    var partnerAppearanceRate = ushort.Parse(partner.Attribute("AppearanceRate")?.Value ?? "0");
                    var partnerCount = ushort.Parse(partner.Attribute("Count")?.Value ?? "0");

                    add.AddPartnerOption(partnerDigimonId, partnerLevel, partnerAppearanceRate, partnerCount);
                }

                Spawns.Add(add);
            }

            Logger.Info($"[RESOURCES] Loaded {Spawns.Count} Spawn data.");
        }

        public static void Reload()
        {
            Spawns.Clear();
            Load();
        }
    }
}