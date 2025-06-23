using System.Xml.Linq;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class PortalDataManager
    {
        private static readonly string path = "Resources/Map/Portals.xml";
        public static List<Portal> Portals { get; private set; } = new();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Portal file [{path}] not found.");

            var doc = XDocument.Load(path);
            var portals = doc.Element("Portals")?.Elements("Portal");

            if (portals is null)
                throw new Exception("Missing <Portals> root.");

            Portals = portals.Select(p => new Portal
            {
                MapID = int.Parse(p.Attribute("MapID")?.Value ?? "0"),
                PosX = short.Parse(p.Attribute("PosX")?.Value ?? "0"),
                PosY = short.Parse(p.Attribute("PosY")?.Value ?? "0"),
                DestMapID = int.Parse(p.Attribute("DestMapID")?.Value ?? "0"),
                DestPosX = short.Parse(p.Attribute("DestPosX")?.Value ?? "0"),
                DestPosY = short.Parse(p.Attribute("DestPosY")?.Value ?? "0"),
            }).ToList();

            Logger.Info($"[RESOURCES] Loaded {Portals.Count} Portal data.");
        }

        public static void Reload()
        {
            Portals.Clear();
            Load();
        }
    }
}