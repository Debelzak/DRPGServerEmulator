using System.Diagnostics;
using DRPGServer.Managers;
namespace DRPGServer
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                ConfigManager.Load();

                await Task.WhenAll(
                    ServerManager.LoginServer.Start(),
                    ServerManager.ChannelServer.Start(),
                    ServerManager.MapServer.Start()
                );
            }
            catch (Exception ex)
            {
                if(ex.InnerException is not null)
                    Logger.Error(ex.InnerException.Message);
                else
                    Logger.Error(ex.Message);
                
                Environment.Exit(1);
            }
        }
    }
}
