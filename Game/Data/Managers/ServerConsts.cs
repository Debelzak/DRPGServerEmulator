using System.Globalization;
using System.Xml.Linq;
using DRPGServer.Game.Entities;

namespace DRPGServer.Game.Data.Managers
{
    public static class ServerConsts
    {
        private static readonly string path = "Resources/ServerConsts.xml";
        private static Dictionary<string, float> serverConsts { get; set; } = new();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"ServerConsts file [{path}] not found.");

            var doc = XDocument.Load(path);
            var consts = doc.Element("ServerConsts")?.Elements("Const");

            if (consts is null)
                throw new Exception("Missing <ServerConsts> root.");

            foreach (var const_ in consts)
            {
                string key = const_.Attribute("Key")?.Value ?? "0";
                float value = float.Parse(const_.Attribute("Value")?.Value ?? "0", CultureInfo.InvariantCulture);
                serverConsts.Add(key, value);
            }
        }

        public static float Get(string key)
        {
            if (!serverConsts.TryGetValue(key, out var result))
                throw new Exception($"Could not find const {key} in ServerConsts.xml");

            return result;
        }

        public static void Reload()
        {
            serverConsts.Clear();
            Load();
        }
    }
}