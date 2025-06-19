using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Managers;

namespace DRPGServer.Network.Handlers.Channel
{
    class AuthenticationHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_AUTH;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var unknown_1 = packet.ReadInt();
            var unknown_2 = packet.ReadInt();
            var username = packet.ReadString(21);
            var authKey = packet.ReadString(40);
            var unknown_3 = packet.ReadBytes(60);
            var mac_1 = packet.ReadString(21);
            var mac_2 = packet.ReadString(21);
            var mac_3 = packet.ReadString(21);

            if (username == "debelzak26") // Placeholder connection success
            {
                var data = new AuthenticationPacket
                {
                    ErrorCode = ERROR_ID.SUCCESS,
                    Username = username,
                };

                client.SessionStart(username);
                client.Send(data);
            }
            else
            {
                var data = new AuthenticationPacket
                {
                    ErrorCode = ERROR_ID.INVALID_CREDENTIALS,
                    Username = username,
                    AuthKey = authKey,
                    Mac1 = mac_1,
                    Mac2 = mac_2,
                    Mac3 = mac_3,
                };
                client.Send(data);
            }
        }
    }
}