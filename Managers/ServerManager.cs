using DRPGServer.Network;
using DRPGServer.Network.Listeners;
namespace DRPGServer.Managers
{
    static class ServerManager
    {
        public static Listener LoginServer { get; private set; } = new LoginListener(ConfigManager.LoginServerConfig.IPAddress, ConfigManager.LoginServerConfig.Port);
        public static Listener ChannelServer { get; private set; } = new ChannelListener(ConfigManager.ChannelServerConfig.IPAddress, ConfigManager.ChannelServerConfig.Port);
        public static Listener MapServer { get; private set; } = new MapListener(ConfigManager.MapServerConfig.IPAddress, ConfigManager.MapServerConfig.Port);
    }
}
