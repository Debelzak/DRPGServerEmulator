using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class AddAbilityPointHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_DIGIMON_ADD_ABILITY_POINT_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var digimon = client.Player?.Character.MainDigimon;
            if (digimon == null) return;

            var digimonUID = packet.ReadUInt();
            var digimonSerial = packet.ReadBytes(16);
            var unk = packet.ReadBytes(668);
            var unk2 = packet.ReadByte(); // Maybe evolution ID.
            var attemptStat = packet.ReadByte();

            if (digimon.AddAbilityPoint(attemptStat))
            {
                var outpacket = new AddAbilityPointPacket(digimon);
                client.Send(outpacket);
            }
        }
    }
}