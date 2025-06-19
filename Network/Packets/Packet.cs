using DRPGServer.Network.Enum;

namespace DRPGServer.Network.Packets
{
    public abstract class Packet(byte[]? data = null) : IDisposable
    {
        public ushort Signature { get; protected set; } = 0x00cc;
        public ushort PacketID { get; protected set; } = 0;
        public uint Size { get; protected set; }
        public uint SequenceNo { get; set; } = 0;
        public ushort CheckSum { get; protected set; }
        protected MemoryStream Stream { get; private set; } = (data == null) ? new() : new(data);
        public abstract byte[] GetBytes();

        public virtual void Dispose()
        {
            Stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}