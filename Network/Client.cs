using System.Net;
using System.Net.Sockets;
using DRPGServer.Game.Entities;
using DRPGServer.Managers;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network
{
    public class Client : IDisposable
    {
        public Guid ConnectionId { get; private set; } = Guid.NewGuid();
        public IPAddress IP { get; }
        public int RemotePort { get; }
        public User? User { get; private set; }
        public Player? Player { get; private set; }
        public SERVER_TYPE ServerType = SERVER_TYPE.UNKNOWN;
        private readonly Socket socket;
        public PacketBuffer PacketBuffer { get; } = new();
        public bool IsDisposed { get; private set; } = false;

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
            if (IsDisposed) throw new ObjectDisposedException(nameof(Client));

            byte[] buffer = packet.GetBytes();

            socket.Send(buffer);
            Logger.DebugPacket(buffer, ServerType);

            packet.Dispose();
        }

        public int Receive(byte[] buffer)
        {
            if (IsDisposed)
            {
                return 0;
            }

            return socket.Receive(buffer);
        }

        public void SessionStart(User user)
        {
            if (User != null) throw new InvalidOperationException("Trying to start an already-started session.");

            SetUser(user);
            
            switch (ServerType)
            {
                case SERVER_TYPE.LOGIN_SERVER: ServerManager.LoginServer.RegisterUsername(user.Username, this); break;
                case SERVER_TYPE.CHANNEL_SERVER: ServerManager.ChannelServer.RegisterUsername(user.Username, this); break;
                case SERVER_TYPE.MAP_SERVER: ServerManager.MapServer.RegisterUsername(user.Username, this); break;
                case SERVER_TYPE.GLOBAL_SERVER: ServerManager.GlobalServer.RegisterUsername(user.Username, this); break;
                default: throw new Exception("Tried to start session within a server that doesn't exist");
            }
        }

        public void SetUser(User user)
        {
            if (User != null) throw new InvalidOperationException("Trying to attach user to a client that already has a user.");
            User = user;
        }

        public void SetPlayer(Player player)
        {
            if (Player != null) throw new InvalidOperationException("Trying to attach player to a client that already has a player.");
            Player = player;
        }

        public async Task DisposeDelayed(int milliseconds)
        {
            if (IsDisposed) return;

            await Task.Delay(milliseconds);

            Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            try
            {
                if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on closing socket: {ex.Message}");
            }

            Player?.Dispose();

            socket.Close();

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}