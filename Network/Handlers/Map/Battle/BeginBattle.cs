using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class BeginBattleHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_BEGIN;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var battle = client.Player?.Battle;
            if (battle == null) return;

            byte[] hash = packet.ReadBytes(16); // Battle serial confirmation sent by client
            byte[] unknown_1 = packet.ReadBytes(16);

            var beginBattlePacket = new BattleBeginPacket();
            client.Send(beginBattlePacket);
            
            if (!battle.IsIdle) battle.Ready();
        }
    }
}