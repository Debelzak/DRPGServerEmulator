using System.Xml.Linq;
using DRPGServer.Game.Data.Models;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class DigimonDataManager
    {
        private static readonly string path = "Resources/Digimon/DigimonData.xml";
        public static Dictionary<int, DigimonTableEntry> DigimonTable { get; private set; } = [];

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

                var toAdd = new DigimonTableEntry()
                {
                    DigimonID = digimonId,
                    Name = digimon.Attribute("Name")?.Value ?? string.Empty,
                    BaseHP = int.Parse(digimon.Attribute("BaseHP")?.Value ?? "0"),
                    BaseVP = int.Parse(digimon.Attribute("BaseVP")?.Value ?? "0"),
                    BaseATK = int.Parse(digimon.Attribute("BaseATK")?.Value ?? "0"),
                    BaseDEF = int.Parse(digimon.Attribute("BaseDEF")?.Value ?? "0"),
                    BaseBR = int.Parse(digimon.Attribute("BaseBR")?.Value ?? "0"),
                    ActionGauge = int.Parse(digimon.Attribute("ActionGauge")?.Value ?? "0"),
                    Classification = byte.Parse(digimon.Attribute("Classification")?.Value ?? "0"),
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