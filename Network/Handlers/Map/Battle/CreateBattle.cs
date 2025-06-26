using DRPGServer.Common;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class CreateBattleHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_START_RES;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var battle = client.Player?.Battle;
            if (battle == null) return;

            byte[] battleSerialInput = packet.ReadBytes(16);
            byte unknown_1 = packet.ReadByte();
            byte unknown_2 = packet.ReadByte();
            short unknown_3 = packet.ReadShort();

            Serial battleSerial = new(battleSerialInput);
            if (battle.Serial.ToString() != battleSerial.ToString()) return;

            var createBattlePacket = new BattleCreatePacket(battle);
            client.Send(createBattlePacket);
        }
    }
}