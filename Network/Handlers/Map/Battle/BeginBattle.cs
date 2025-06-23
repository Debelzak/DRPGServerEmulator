using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map.Battle;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class BeginBattleHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_BEGIN;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            byte[] hash = packet.ReadBytes(16);
            byte[] unknown_1 = packet.ReadBytes(16);

            var beginBattlePacket = new BeginBattlePacket();
            client.Send(beginBattlePacket);

            //var AttackReadyPacket = new AttackReadyPacket();
            //client.Send(AttackReadyPacket);
        }
    }
}