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
                var DisplayDigimonID = ushort.Parse(spawn.Attribute("DisplayDigimonID")?.Value ?? "0");
                if (DisplayDigimonID == 0) continue;

                if (!DigimonDataManager.DigimonTable.TryGetValue(DisplayDigimonID, out var digimon))
                {
                    throw new Exception($"[SpawnDataManager] Digimon ID [{DisplayDigimonID}] was not found in DigimonData");
                }

                var add = new DigimonSpawn(digimon.DigimonID)
                {
                    MapID = byte.Parse(spawn.Attribute("MapID")?.Value ?? "0"),
                    PosXMin = short.Parse(spawn.Attribute("PosXMin")?.Value ?? "0"),
                    PosXMax = short.Parse(spawn.Attribute("PosXMax")?.Value ?? "0"),
                    PosYMin = short.Parse(spawn.Attribute("PosYMin")?.Value ?? "0"),
                    PosYMax = short.Parse(spawn.Attribute("PosYMax")?.Value ?? "0"),
                    MaxCount = short.Parse(spawn.Attribute("MaxCount")?.Value ?? "0"),
                    RespawnTime = short.Parse(spawn.Attribute("RespawnTime")?.Value ?? "0")
                };

                var options = spawn.Elements("SpawnOption");
                if (!options.Any())
                {
                    throw new Exception($"[SpawnDataManager] Spawn Digimon ID [{DisplayDigimonID}] in MapID [{add.MapID}] has no spawn options!");
                }

                foreach (var option in options)
                {
                    var DigimonId = ushort.Parse(option.Attribute("DigimonID")?.Value ?? "0");
                    if (!DigimonDataManager.DigimonTable.TryGetValue(DigimonId, out var optionDigimon))
                    {
                        throw new Exception($"[SpawnDataManager] Spawn Option Digimon ID [{DigimonId}] was not found in DigimonData");
                    }

                    var Level = ushort.Parse(option.Attribute("Level")?.Value ?? "0");
                    var STR = ushort.Parse(option.Attribute("STR")?.Value ?? "0");
                    var AGI = ushort.Parse(option.Attribute("AGI")?.Value ?? "0");
                    var CON = ushort.Parse(option.Attribute("CON")?.Value ?? "0");
                    var INT = ushort.Parse(option.Attribute("INT")?.Value ?? "0");
                    var AppearanceRate = ushort.Parse(option.Attribute("AppearanceRate")?.Value ?? "0");
                    var expReward = long.Parse(option.Attribute("ExpReward")?.Value ?? "0");
                    var bitReward = double.Parse(option.Attribute("BitReward")?.Value ?? "0");

                    add.AddSpawnOption(DigimonId, Level, STR, AGI, CON, INT, expReward, bitReward, AppearanceRate);
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