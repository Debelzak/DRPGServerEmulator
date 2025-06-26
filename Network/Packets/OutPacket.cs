using System.Runtime.Intrinsics.Arm;
using DRPGServer.Network.Enum;

namespace DRPGServer.Network.Packets
{
    public abstract class OutPacket : Packet
    {
        private readonly BinaryWriter writer;

        public OutPacket(ushort packetId) : base()
        {
            PacketID = packetId;
            writer = new(Stream);
        }

        public void WriteByte(byte data)
        {
            writer.Write(data);
        }

        public void WriteBytes(byte[] data)
        {
            writer.Write(data);
        }

        public void WriteInt(int value)
        {
            writer.Write(value);
        }

        public void WriteUInt(uint value)
        {
            writer.Write(value);
        }

        public void WriteLong(long value)
        {
            writer.Write(value);
        }

        public void WriteULong(ulong value)
        {
            writer.Write(value);
        }

        public void WriteShort(short value)
        {
            writer.Write(value);
        }

        public void WriteUShort(ushort value)
        {
            writer.Write(value);
        }

        public void WriteDouble(double value)
        {
            writer.Write(value);
        }

        public void WriteString(string value, int length)
        {
            writer.Write(Utils.StringToFixedBytes(value, length));
        }

        public override byte[] GetBytes()
        {
            Stream.Position = 0;
            Serialize();

            Size = (ushort)(14 + Stream.Length);

            List<byte> fullPacket =
            [
                .. BitConverter.GetBytes(Signature),
                .. BitConverter.GetBytes(PacketID),
                .. BitConverter.GetBytes(Size),
                .. BitConverter.GetBytes(SequenceNo),
                .. BitConverter.GetBytes(CRC16.Compute(Stream.ToArray())),
                .. Stream.ToArray(),
            ];

            return fullPacket.ToArray();
        }

        protected abstract void Serialize();

        public override void Dispose()
        {
            base.Dispose();
            writer.Dispose();
        }
    }
}