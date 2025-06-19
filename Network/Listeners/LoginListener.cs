using System.Net;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network.Listeners
{
    class LoginListener(string ipAddress, int port) : Listener(ipAddress, port, SERVER_TYPE.LOGIN_SERVER)
    {
        public override void TryProcessPacket(byte[] data, Client client)
        {
            try
            {
                InPacket packet = new(data);

                if (!packet.IsValid)
                {
                    packet.Dispose();
                    return;
                }

                Logger.Debug($"[{serverType}] Receiving packet {(Enum.Login.PACKET_ID)packet.PacketID} ({(int)packet.PacketID}) from {client.IP}:{client.RemotePort}.");
                Logger.DebugPacket(data, serverType, true);

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
    }
}