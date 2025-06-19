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

        public void Write(byte data)
        {
            writer.Write(data);
        }

        public void Write(byte[] data)
        {
            writer.Write(data);
        }

        public void Write(int value)
        {
            writer.Write(value);
        }

        public void Write(uint value)
        {
            writer.Write(value);
        }

        public void Write(short value)
        {
            writer.Write(value);
        }

        public void Write(ushort value)
        {
            writer.Write(value);
        }

        public void Write(string value, int length)
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