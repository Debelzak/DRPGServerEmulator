using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map.Character;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Network.Packets.Map.Battle;
using DRPGServer.Game.Enum;
using DRPGServer.Game.Entities;

namespace DRPGServer.Network.Handlers.Map
{
    class MoveCharacterHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_MOVE_CHARACTER_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var player = client.Player;
            if (player is null)
            {
                client.Dispose(); return;
            }

            int unknown_1 = packet.ReadShort();
            short positionX = packet.ReadShort();
            short positionY = packet.ReadShort();

            player.ChangePosition(positionX, positionY);

            var data = new MoveCharacterPacket();
            client.Send(data);
            
            foreach (var portal in player.Zone.Portals)
            {
                if (portal.AffectsPosition(positionX, positionY))
                {
                    player.ChangeLocation((byte)portal.DestMapID, portal.DestPosX, portal.DestPosY);
                }
            }
        }
    }
}