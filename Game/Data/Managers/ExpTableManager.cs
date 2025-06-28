using System.Xml.Linq;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class ExpTableManager
    {
        private static readonly string pathDigimon = "Resources/Digimon/ExpTable.xml";
        private static readonly string pathCharacter = "Resources/Character/ExpTable.xml";

        public static Dictionary<int, long> DigimonExpTable { get; private set; } = new();
        public static Dictionary<int, long> CharacterExpTable { get; private set; } = new();
        public static void Load()
        {
            LoadDigimon();
            LoadCharacter();
        }

        private static void LoadDigimon()
        {
            if (!File.Exists(pathDigimon))
                throw new FileNotFoundException($"DigimonExpTable file [{pathDigimon}] not found.");
                
            var doc = XDocument.Load(pathDigimon);
            var expTable = doc.Element("DigimonExpTable")?.Elements("Digimon");

            if (expTable is null)
                throw new Exception("Missing <DigimonExpTable> root.");

            foreach (var digimon in expTable)
            {
                var levelValue = ushort.Parse(digimon.Attribute("Level")?.Value ?? "0");
                var expValue = long.Parse(digimon.Attribute("TargetEXP")?.Value ?? "0");
                if (!DigimonExpTable.TryGetValue(levelValue, out _))
                {
                    DigimonExpTable.Add(levelValue, expValue);
                }
            }
        }

        private static void LoadCharacter()
        {
            if (!File.Exists(pathDigimon))
                throw new FileNotFoundException($"CharacterExpTable file [{pathCharacter}] not found.");
                
            var doc = XDocument.Load(pathCharacter);
            var expTable = doc.Element("CharacterExpTable")?.Elements("Character");

            if (expTable is null)
                throw new Exception("Missing <CharacterExpTable> root.");

            foreach (var character in expTable)
            {
                var levelValue = ushort.Parse(character.Attribute("Level")?.Value ?? "0");
                var expValue = long.Parse(character.Attribute("TargetEXP")?.Value ?? "0");
                if (!CharacterExpTable.TryGetValue(levelValue, out _))
                {
                    CharacterExpTable.Add(levelValue, expValue);
                }
            }
        }

        public static void Reload()
        {
            DigimonExpTable.Clear();
            Load();
        }
    }
}