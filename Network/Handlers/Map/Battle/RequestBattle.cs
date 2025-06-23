using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map.Battle;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class RequestBattleHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_START_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            int unknown_1 = packet.ReadInt();
            byte[] hash = packet.ReadBytes(16);

            var startBattlePacket = new StartBattlePacket();
            client.Send(startBattlePacket);
        }
    }
}