using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Global;

namespace DRPGServer.Network.Listeners
{
    class GlobalListener(string ipAddress, int port) : Listener(ipAddress, port, SERVER_TYPE.GLOBAL_SERVER)
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

                Logger.Debug($"[{serverType}] Receiving packet {(Enum.Global.PACKET_ID)packet.PacketID} ({(int)packet.PacketID}) from {client.IP}:{client.RemotePort}.");
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
            client.Send(new _0x0e_Packet());
        }
    }
}