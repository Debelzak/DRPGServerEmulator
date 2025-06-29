using System.Xml.Linq;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Managers;

namespace DRPGServer
{
    public class LoginServerConfig
    {
        public string IPAddress = string.Empty;
        public int Port;
    }

    public class ChannelServerConfig
    {
        public string IPAddress = string.Empty;
        public int Port;
    }

    public class MapServerConfig
    {
        public string IPAddress = string.Empty;
        public int Port;
    }

    public class GlobalServerConfig
    {
        public string IPAddress = string.Empty;
        public int Port;
    }

    public static class ConfigManager
    {
        public static bool DebugMode { get; private set; }
        public static bool DebugPackets { get; private set; }

        public static LoginServerConfig LoginServerConfig { get; private set; } = new();
        public static ChannelServerConfig ChannelServerConfig { get; private set; } = new();
        public static MapServerConfig MapServerConfig { get; private set; } = new();
        public static GlobalServerConfig GlobalServerConfig { get; private set; } = new();

        private static void Load(string path = "Config.xml")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Config file [{path}] not found.");

            var doc = XDocument.Load(path);
            var config = doc.Element("Config");
            if (config is null)
                throw new Exception("Missing <Config> root.");

            DebugMode = bool.TryParse(config.Element("DebugMode")?.Value, out var d1) && d1;
            DebugPackets = bool.TryParse(config.Element("DebugPackets")?.Value, out var d2) && d2;

            ParseServer(config.Element("LoginServer"), LoginServerConfig);
            ParseServer(config.Element("ChannelServer"), ChannelServerConfig);
            ParseServer(config.Element("MapServer"), MapServerConfig);
            ParseServer(config.Element("GlobalServer"), GlobalServerConfig);
        }

        public static void LoadAll()
        {
            Load();
            DigimonDataManager.Load();
            ItemTableManager.Load();
            ExpTableManager.Load();
            PortalDataManager.Load();
            SpawnDataManager.Load();
            ZoneManager.Load();
        }

        private static void ParseServer(XElement? element, dynamic server)
        {
            if (element == null) return;

            var ip = element.Attribute("BindIP")?.Value;
            if (!string.IsNullOrWhiteSpace(ip))
                server.IPAddress = ip;

            var portStr = element.Attribute("Port")?.Value;
            if (int.TryParse(portStr, out var port))
                server.Port = port;
        }
    }
}
