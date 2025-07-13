using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Game.Data.DAOs;

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

            var account = AccountDAO.GetAccountByUsername(username);
            if (account != null && account.AuthKey == authKey)
            {
                client.SessionStart(account); // Placeholder

                var characters = CharacterDAO.GetAccountCharacters(account.UID);

                for (byte i = 0; i < 4; i++)
                {
                    client.User?.SetCharacterSlot(i, characters[i]);
                }

                var data = new AuthenticationPacket
                {
                    ErrorCode = (int)ERROR_ID.SUCCESS,
                    Username = username,
                };

                client.Send(data);
            }
            else
            {
                var data = new AuthenticationPacket
                {
                    ErrorCode = (int)ERROR_ID.INVALID_CREDENTIALS,
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