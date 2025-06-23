using System.Net;
using System.Net.Sockets;
using System.Reflection;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Handlers;
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

        public virtual void TryProcessPacket(InPacket packet, Client client) { }

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
            Logger.Info($"[{serverType}] Connection closed from {client.IP}");
        }

        public void RegisterUsername(string username, Client client)
        {
            if (!connectedUsers.TryAdd(username, client))
                Logger.Error($"[{serverType}] Unable to register [{username}] to client connection id [{client.ConnectionId}].");
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

                connectedClients.TryAdd(client.ConnectionId, client);

                _ = Task.Run(() => HandleClient(client));
            }
        }

        public void HandleClient(Client client)
        {
            byte[] buffer = new byte[2048];

            try
            {
                while (true)
                {
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead == 0) break;

                    byte[] received = buffer[..bytesRead];
                    client.PacketBuffer.Append(received);

                    var packets = client.PacketBuffer.ExtractPackets();
                    
                    foreach (var packet in packets)
                    {
                        TryProcessPacket(packet, client); // Agora direto com InPacket
                    }
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
                if (client.User?.Username is not null)
                {
                    if(!connectedUsers.TryRemove(client.User.Username, out _))
                        Logger.Error($"[{serverType}] Unable to unregister user [{client.User.Username}] from client connection id {client.ConnectionId}.");
                }

                if(!connectedClients.TryRemove(client.ConnectionId, out _))
                    Logger.Error($"[{serverType}] Unable to unregister client connection id [{client.ConnectionId}].");

                OnClientDisconnected(client);
                client.Dispose();
            }
        }
    }
}