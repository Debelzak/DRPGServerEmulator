using System.Diagnostics;
using DRPGServer.Network.Listeners;
namespace DRPGServer.Managers
{
    static class ServerManager
    {
        public static LoginListener LoginServer { get; private set; } = new LoginListener(ConfigManager.LoginServerConfig.IPAddress, ConfigManager.LoginServerConfig.Port);
        public static ChannelListener ChannelServer { get; private set; } = new ChannelListener(ConfigManager.ChannelServerConfig.IPAddress, ConfigManager.ChannelServerConfig.Port);
        public static MapListener MapServer { get; private set; } = new MapListener(ConfigManager.MapServerConfig.IPAddress, ConfigManager.MapServerConfig.Port);
        public static GlobalListener GlobalServer { get; private set; } = new GlobalListener(ConfigManager.GlobalServerConfig.IPAddress, ConfigManager.GlobalServerConfig.Port);

        public static async Task Start()
        {
            var cts = new CancellationTokenSource();

            try
            {
                ConfigManager.LoadAll();

                // Start game logic loop em paralelo
                var serverTick = StartGameLoopAsync(cts.Token);

                await Task.WhenAll(
                    LoginServer.Start(),
                    ChannelServer.Start(),
                    MapServer.Start(),
                    GlobalServer.Start(),
                    serverTick
                );
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null)
                    Logger.Error(ex.InnerException.Message);
                else
                    Logger.Error(ex.Message);

                Environment.Exit(1);
            }
        }
        
        static async Task StartGameLoopAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            double lastUpdate = stopwatch.Elapsed.TotalMilliseconds;
            const double tickRateMs = 50; // 20 updates per second (~50ms)

            while (!cancellationToken.IsCancellationRequested)
            {
                double now = stopwatch.Elapsed.TotalMilliseconds;
                double deltaTime = now - lastUpdate;

                if (deltaTime >= tickRateMs)
                {
                    lastUpdate = now;
                    ZoneManager.UpdateAll(deltaTime);
                }

                await Task.Delay(1, cancellationToken);
            }
        }
    }
}
