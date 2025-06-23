using System.Diagnostics;
using System.Xml.Linq;
using DRPGServer.Game;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Game.Entities;
using DRPGServer.Network.Packets;

namespace DRPGServer.Managers
{
    public static class ZoneManager
    {
        public static Dictionary<byte, Zone> Zones { get; private set; } = new();

        public static void UpdateAll(double deltaTime)
        {
            foreach (var zone in Zones.Values)
            {
                zone.Update(deltaTime);
            }
        }

        public static Zone? GetZoneByMapID(byte mapId)
        {
            Zones.TryGetValue(mapId, out var result);
            return result;
        }

        public static void BroadcastToAll(OutPacket packet)
        {
            foreach (var zone in Zones.Values)
            {
                zone.Broadcast(packet);
            }
        }

        public static bool TransferPlayer(Player player, byte mapId)
        {
            var oldZone = player.Zone;
            if (oldZone.MapID == mapId) return true;

            var newZone = GetZoneByMapID(mapId);
            if (newZone == null) return false;

            oldZone.RemovePlayer(player);
            newZone.AddPlayer(player);
            player.Zone = newZone;
            return true;
        }

        public static void Load(string path = "Resources/Map/Zones.xml")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"S file [{path}] not found.");

            var doc = XDocument.Load(path);
            var zones = doc.Element("Zones")?.Elements("Zone");

            if (zones is null)
                throw new Exception("Missing <Zones> root.");

            foreach (var zone in zones)
            {
                var mapId = byte.Parse(zone.Attribute("MapID")?.Value ?? "0");
                if (mapId == 0) continue;

                var toAdd = new Zone(mapId);

                if (Zones.TryAdd(mapId, toAdd))
                    toAdd.Start();
            }

            Logger.Info($"[RESOURCES] Loaded {Zones.Count} Zones.");
        }
    }
}