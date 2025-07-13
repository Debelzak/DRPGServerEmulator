using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class RefreshCharacterInfoHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_REFRESH_CHARACTER_INFO_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var player = client.Player;
            if (player == null) {
                client.Dispose(); return;
            }

            // Character session key passed by client
            byte[] key = packet.ReadBytes(16);

            var data = new RefreshCharacterInfoPacket
            {
                CharacterKey = player.Character.Serial.Data,
                Nickname = player.Character.Name,
                TamerID = player.Character.TamerID,
                PositionX = player.Character.PositionX,
                PositionY = player.Character.PositionY,
                DigimonKey = player.Character.MainDigimon.Serial.Data,
                DigimonID = player.Character.MainDigimon.DigimonID,
                DigimonNickname = player.Character.MainDigimon.Name,
            };
            
            client.Send(data);
        }
    }
}