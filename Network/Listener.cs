using System.Net;
using System.Net.Sockets;
using System.Reflection;
using DRPGServer.Network.Enum;
using System.Collections.Concurrent;
using System.Data;

namespace DRPGServer.Network
{
    abstract class Listener
    {
        public IPAddress IP { get; private set; }
        public int Port { get; private set; }
        public Socket Socket { get; private set; }

        protected readonly ConcurrentDictionary<Guid, Client> connectedClients = new();
        protected readonly ConcurrentDictionary<string, Client> connectedUsers = new();
        public IReadOnlyCollection<Client> GetConnectedUsers() => [.. connectedUsers.Values];
        protected readonly PacketDispatcher packetDispatcher = new();
        protected readonly SERVER_TYPE serverType;

        public Listener(string ipAddress, int port, SERVER_TYPE serverType)
        {
            if (!IPAddress.TryParse(ipAddress, out var parsedIP))
                throw new FormatException($"Invalid IP address: '{ipAddress}'");

            IP = parsedIP;
            Port = port;
            this.serverType = serverType;

            AutoRegisterHandlers();

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.NoDelay = true;
            Socket.Bind(new IPEndPoint(IP, port));
            Socket.Listen(backlog: 100);
        }

        public virtual void TryProcessPacket(byte[] data, Client client) { }

        private void AutoRegisterHandlers()
        {
            var handlerType = typeof(IPacketHandler);
            var handlers = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => !t.IsAbstract && handlerType.IsAssignableFrom(t));

            foreach (var type in handlers)
            {
                if (Activator.CreateInstance(type) is IPacketHandler instance)
                {
                    packetDispatcher.Register(instance.ServerType, instance.Opcode, instance);
                }
            }
        }

        protected virtual void OnClientConnected(Client client)
        {
            Logger.Info($"[{serverType}] Accepted connection request from {client.IP}.");
        }

        protected virtual void OnClientDisconnected(Client client)
        {
            Logger.Info($"[{serverType}] Connection closed with {client.IP}");
        }

        public void RegisterUsername(string username, Client client)
        {
            if (!connectedUsers.TryAdd(username, client))
            {
                throw new InvalidOperationException($"Trying to register an already registered/connected user: {username}");
            }
            Logger.Debug($"[{serverType}] Registering username [{username}] to client connection id [{client.ConnectionId}].");
        }

        public Client? GetClientByUsername(string username)
        {
            connectedUsers.TryGetValue(username, out var client);
            return client;
        }

        public async Task Start()
        {
            Logger.Info($"[{serverType}] Listening at {IP}:{Port}");

            while (true)
            {
                var socket = await Socket.AcceptAsync();
                var client = new Client(socket, serverType);
                
                OnClientConnected(client);

                Logger.Debug($"[{serverType}] Registering new client connection [{client.ConnectionId}]");
                connectedClients.TryAdd(client.ConnectionId, client);

                _ = Task.Run(() => HandleClient(client));
            }
        }

        public void HandleClient(Client client)
        {
            byte[] buffer = new byte[16384];

            try
            {
                while (true)
                {
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead == 0) break;

                    TryProcessPacket(buffer[..bytesRead], client);
                }
            }
            catch (SocketException socketException)
            {
                if (socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Logger.Info($"[{serverType}] Client {client.IP} disconnected.");
                }
                else
                {
                    Logger.Error($"[{serverType}] SocketException: {socketException.Message}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"[{serverType}] Client failure: {ex.Message}");
            }
            finally
            {
                if (client.Session?.Username is not null)
                {
                    connectedUsers.TryRemove(client.Session.Username, out _);
                    Logger.Debug($"[{serverType}] Unregistering [{client.Session.Username}] from client connection id {client.ConnectionId}.");
                }

                connectedClients.TryRemove(client.ConnectionId, out _);
                Logger.Debug($"[{serverType}] Unregistering client connection [{client.ConnectionId}].");

                OnClientDisconnected(client);
                client.Dispose();
            }
        }
    }
}