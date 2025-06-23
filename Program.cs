using DRPGServer.Managers;

namespace DRPGServer
{
    class Program
    {
        static async Task Main()
        {
            await ServerManager.Start();
        }
    }
}
