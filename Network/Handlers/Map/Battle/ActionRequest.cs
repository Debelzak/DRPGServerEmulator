using DRPGServer.Common;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network.Handlers.Map.Battle
{
    class ActionRequestHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_BATTLE_ACTION_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var battle = client.Player?.Battle;
            if (battle == null) return;

            byte unk1 = packet.ReadByte();
            ushort unk2 = packet.ReadUShort();
            byte unk3 = packet.ReadByte();
            byte[] requesterSerialInput = packet.ReadBytes(16);
            byte[] targetSerialInput = packet.ReadBytes(16);

            Serial requesterSerial = new(requesterSerialInput);
            Serial targetSerial = new(targetSerialInput);

            battle.ActionRequest(requesterSerial.ToString(), targetSerial.ToString());
        }
    }
}