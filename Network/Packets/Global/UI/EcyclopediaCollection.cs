using DRPGServer.Network.Enum.Global;

namespace DRPGServer.Network.Packets.Global.UI
{
    class EncyclopediaCollectionPacket() : OutPacket((ushort)PACKET_ID.GLOBAL_ENCYCLOPEDIA_COLLECTION)
    {
        public byte[] Key1 { get; set; } = new byte[16];
        public byte[] Key2 { get; set; } = new byte[16];
        protected override void Serialize()
        {
            WriteBytes(Key1);
            WriteBytes([0x8f, 0x11, 0x4e, 0x2f, 0xcb, 0x44, 0xe0, 0x66, 0x89, 0x74, 0x1f, 0x78, 0xa0, 0x4e, 0xf1, 0xc9]); // Need to change ?
            for (int i = 0; i < 2; i++)
            {
                WriteBytes(Key2);
            }
        }
    }
}