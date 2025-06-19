namespace DRPGServer.Network
{
    class Session(string username, Client client) : IDisposable
    {
        public string Username { get; private set; } = username;
        public Client Client { get; private set; } = client;

        public void Dispose()
        {
            
        }
    }
}