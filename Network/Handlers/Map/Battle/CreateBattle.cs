using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map.Battle;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class CreateBattleHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_START_RES;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            byte[] hash = packet.ReadBytes(16);
            byte unknown_1 = packet.ReadByte();
            byte unknown_2 = packet.ReadByte();
            short unknown_3 = packet.ReadShort();

            var createBattlePacket = new CreateBattlePacket();
            client.Send(createBattlePacket);
        }
    }
}