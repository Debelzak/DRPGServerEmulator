using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterListHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_LIST;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            var data = new CharacterListPacket();
            client.Send(data);
        }
    }
}

/*
 * Input Packet: CHARACTER_LIST (0x0012)
 * Total Size: 0x001C (28 bytes)
 * Header (14 bytes)
 * ┌────────┬────────────┬────────────────────────────┐
 * │ Offset │ Field      │ Description                │
 * ├────────┼────────────┼────────────────────────────┤
 * │ 0x00   │ ushort     │ Signature          (0x00CC)│
 * │ 0x02   │ ushort     │ Opcode             (0x0012)│
 * │ 0x04   │ ushort     │ Size               (0x001C)│
 * │ 0x06   │ int        │ Unknown_1      (0x00000000)│
 * │ 0x0A   │ ushort     │ Unknown_2          (0x0000)│
 * │ 0x0C   │ ushort     │ CRC16?             (0x0000)│
 * └────────┴────────────┴────────────────────────────┘
 * Payload (14 bytes)
 * ┌────────┬────────────┬────────────────────────────┐
 * │ 0x0E   │ byte[14]   │ Unknown_1                  │
 * └────────┴────────────┴────────────────────────────┘
 * ============================================================
 *
 * Output Packet: CHARACTER_LIST (0x0012)
 * Total Size: 0x001C (514 bytes)
 * Header (14 bytes)
 * ┌────────┬────────────┬────────────────────────────┐
 * │ Offset │ Field      │ Description                │
 * ├────────┼────────────┼────────────────────────────┤
 * │ 0x00   │ ushort     │ Signature          (0x00CC)│
 * │ 0x02   │ ushort     │ Opcode             (0x0012)│
 * │ 0x04   │ ushort     │ Size               (0x001C)│
 * │ 0x06   │ int        │ Unknown_1      (0x00000000)│
 * │ 0x0A   │ ushort     │ Unknown_2          (0x0000)│
 * │ 0x0C   │ ushort     │ CRC16?             (0x0000)│
 * └────────┴────────────┴────────────────────────────┘
 * Payload (500 bytes)
 * ┌────────┬────────────┬────────────────────────────┐
 * │ 0x0E   │ byte[500]  │ Reserved / Unknown         │
 * └────────┴────────────┴────────────────────────────┘
 */