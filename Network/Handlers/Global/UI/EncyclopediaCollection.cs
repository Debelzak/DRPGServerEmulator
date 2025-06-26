using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Global;
using DRPGServer.Network.Packets.Global;

namespace DRPGServer.Network.Handlers.Global.UI
{
    class EncyclopediaCollectionHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.GLOBAL_ENCYCLOPEDIA_COLLECTION;
        public SERVER_TYPE ServerType => SERVER_TYPE.GLOBAL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            byte[] key_1 = [0xA5, 0xEF, 0xC3, 0x17, 0x40, 0x2A, 0xD9, 0x90, 0xEF, 0x76, 0xB2, 0x13, 0xCF, 0x81, 0xA8, 0x12];
            byte[] key_2 = [0x33, 0x09, 0x00, 0x87, 0xC7, 0xBF, 0xBA, 0xC2, 0xFA, 0x66, 0x7C, 0x07, 0x31, 0xD9, 0xFE, 0x1B];

            var encyclopediaCollection = new EncyclopediaCollectionPacket()
            {
                Key1 = key_1,
                Key2 = key_2,
            };
            
            client.Send(encyclopediaCollection);
        }
    }
}