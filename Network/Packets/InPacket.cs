using System.Text;
using DRPGServer.Network.Enum;

namespace DRPGServer.Network.Packets
{
    public class InPacket : Packet
    {
        private readonly BinaryReader reader;
        public bool IsValid { get; private set; }

        private int remainingToRead { get { return (int)(Stream.Length - Stream.Position); } }

        public InPacket(byte[] data) : base(data)
        {
            reader = new(Stream);

            IsValid = false;

            // Validate if has at least header size
            if (Stream.Length < 14)
                return;

            // Validate signature
            Signature = ReadUShort();
            if (Signature != 0x00cc)
                return;

            // Read packet header
            PacketID = ReadUShort();
            Size = ReadUInt();
            SequenceNo = ReadUInt();
            CheckSum = ReadUShort();

            // Validate packet size
            int contentLength = (int)(Stream.Length - 14);
            if (Size != data.Length)
                return;

            // Validade checksum                <- Broken. Client sometimes sends wrong CRC. Not even official server validates checksum.
            //byte[] contentBuffer = new byte[contentLength];
            //Buffer.BlockCopy(data, 14, contentBuffer, 0, contentLength);
            //if (CheckSum != CRC16.Compute(contentBuffer))
            //  return; 

            IsValid = true;
        }

        public byte ReadByte()
        {
            if (remainingToRead < 1) throw new IndexOutOfRangeException();
            return reader.ReadByte();
        }

        public byte[] ReadBytes(int length)
        {
            if (remainingToRead < length) throw new IndexOutOfRangeException();
            return reader.ReadBytes(length);
        }

        public int ReadInt()
        {
            if (remainingToRead < 4) throw new IndexOutOfRangeException();
            return reader.ReadInt32();
        }

        public uint ReadUInt()
        {
            if (remainingToRead < 4) throw new IndexOutOfRangeException();
            return reader.ReadUInt32();
        }

        public short ReadShort()
        {
            if (remainingToRead < 2) throw new IndexOutOfRangeException();
            return reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            if (remainingToRead < 2) throw new IndexOutOfRangeException();
            return reader.ReadUInt16();
        }

        public string ReadString(int length)
        {
            if (remainingToRead < length) throw new IndexOutOfRangeException();
            byte[] buffer = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(buffer)
                    .Replace("\0", string.Empty);
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = Stream.ToArray();

            byte[] payload = buffer.Length > 14 ? buffer[14..] : [];

            List<byte> fullPacket =
            [
                .. BitConverter.GetBytes(Signature),      // 2 bytes
                .. BitConverter.GetBytes(PacketID), // 2 bytes
                .. BitConverter.GetBytes(Size),           // 4 bytes
                .. BitConverter.GetBytes(SequenceNo),      // 4 bytes
                .. BitConverter.GetBytes(CheckSum),       // 2 bytes
                .. payload,                               // N bytes
            ];

            return [.. fullPacket];
        }

        public override void Dispose()
        {
            base.Dispose();
            reader.Dispose();
        }
    }
}