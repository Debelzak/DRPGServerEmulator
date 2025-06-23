using DRPGServer.Game.Entities;
using DRPGServer.Managers;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Listeners
{
    class MapListener(string ipAddress, int port) : Listener(ipAddress, port, SERVER_TYPE.MAP_SERVER)
    {
        public override void TryProcessPacket(InPacket packet, Client client)
        {
            try
            {
                if (!packet.IsValid)
                {
                    Logger.Warn($"[{serverType}] Ignoring invalid packet {(Enum.Channel.PACKET_ID)packet.PacketID} ({(int)packet.PacketID}) from {client.IP}:{client.RemotePort}.");
                    packet.Dispose();
                    return;
                }

                Logger.Debug($"[{serverType}] Receiving packet {(Enum.Map.PACKET_ID)packet.PacketID} ({(int)packet.PacketID}) from {client.IP}:{client.RemotePort}.");
                Logger.DebugPacket(packet.GetBytes(), serverType, true);

                if (!packetDispatcher.Dispatch(packet, client))
                {
                    Logger.Warn($"[{serverType}] There is no processor for packet id '{(int)packet.PacketID}'.");
                }

                packet.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error($"[{serverType}] Error while processing packet: {ex.Message}");
            }
        }

        protected override void OnClientConnected(Client client)
        {
            base.OnClientConnected(client);
            client.Send(new _0x0e_Packet()); // Envia o packet de "hello" ou similar
        }

        public User? GetChannelUser(string username)
        {
            var client = ServerManager.ChannelServer.GetClientByUsername(username);
            return client?.User;
        }
    }
}