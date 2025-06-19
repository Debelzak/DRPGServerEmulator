using System.Net;
using System.Net.Sockets;
using DRPGServer.Managers;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network
{
    class Client : IDisposable
    {
        public Guid ConnectionId { get; private set; } = Guid.NewGuid();
        public IPAddress IP { get; }
        public int RemotePort { get; }
        public Session? Session { get; private set; } = null;
        public SERVER_TYPE ServerType = SERVER_TYPE.UNKNOWN;
        private readonly Socket socket;
        private bool disposed = false;

        public Client(Socket socket, SERVER_TYPE serverType)
        {
            ServerType = serverType;
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));

            if (socket.RemoteEndPoint is not IPEndPoint ipEndPoint)
                throw new ArgumentException("Socket's remote endpoint is not an IPEndPoint.");
            
            IP = ipEndPoint.Address;
            RemotePort = ipEndPoint.Port;
        }

        public void Send(OutPacket packet)
        {
            if (disposed) throw new ObjectDisposedException(nameof(Client));

            byte[] buffer = packet.GetBytes();

            socket.Send(buffer);
            Logger.DebugPacket(buffer, ServerType);

            packet.Dispose();
        }

        public int Receive(byte[] buffer)
        {
            if (disposed)
            {
                return 0;
            }

            return socket.Receive(buffer);
        }

        public void SessionStart(string username)
        {
            Session ??= new Session(username, this);
            switch (ServerType)
            {
                case SERVER_TYPE.LOGIN_SERVER: ServerManager.LoginServer.RegisterUsername(username, this); break;
                case SERVER_TYPE.CHANNEL_SERVER: ServerManager.ChannelServer.RegisterUsername(username, this); break;
                case SERVER_TYPE.MAP_SERVER: ServerManager.MapServer.RegisterUsername(username, this); break;
                //case SERVER_TYPE.GLOBAL_SERVER: ServerManager.GlobalServer.RegisterUsername(username, this); break;
                default: throw new Exception("Tried to start session within a server that doesn't exist");
            }
        }

        public void SessionDestroy()
        {
            Session?.Dispose();
            Session = null;
        }

        public async Task DisposeDelayed(int milliseconds)
        {
            if (disposed) return;

            await Task.Delay(milliseconds);

            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;

            try
            {
                if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on closing socket: {ex.Message}");
            }

            socket.Close();
            SessionDestroy();

            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}