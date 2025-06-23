using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;
using DRPGServer.Network.Packets.Map.Character;

namespace DRPGServer.Network.Handlers.Map
{
    class RefreshCharacterInfoHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_REFRESH_CHARACTER_INFO_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var user = client.User;
            if (user is null) {
                client.Dispose(); return;
            }

            var connectedCharacter = client.Player?.Character;
            if (connectedCharacter is null) {
                client.Dispose(); return;
            }

            // Character session key passed by client
            byte[] key = packet.ReadBytes(16);

            var data = new RefreshCharacterInfoPacket
            {
                CharacterKey = Utils.GenerateRandomSessionId(false),
                Nickname = connectedCharacter.Nickname,
                TamerID = connectedCharacter.TamerID,
                PositionX = connectedCharacter.PositionX,
                PositionY = connectedCharacter.PositionY,
                DigimonKey = Utils.GenerateRandomSessionId(true),
                DigimonID = connectedCharacter.DigimonID,
                DigimonNickname = connectedCharacter.DigimonNickname,
            };
            
            client.Send(data);
        }
    }
}