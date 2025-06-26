using DRPGServer.Network.Enum.Global;

namespace DRPGServer.Network.Packets.Global
{
    class EncyclopediaDataPacket() : OutPacket((ushort)PACKET_ID.GLOBAL_ENCYCLOPEDIA_DATA)
    {
        public byte[] Key1 { get; set; } = new byte[16];
        public byte[] Key2 { get; set; } = new byte[16];
        protected override void Serialize()
        {
            WriteBytes(Key1);
            for (int i = 0; i < 69; i++)
            {
                WriteBytes(Key2);
            }
        }
    }
}