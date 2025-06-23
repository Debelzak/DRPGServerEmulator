namespace DRPGServer.Network.Packets
{
    public class PacketBuffer
    {
        private List<byte> buffer = new();

        public void Append(byte[] data)
        {
            buffer.AddRange(data);
        }

        public List<InPacket> ExtractPackets()
        {
            List<InPacket> packets = new();

            while (buffer.Count >= 14)
            {
                ushort signature = BitConverter.ToUInt16(buffer.ToArray(), 0);
                if (signature != 0x00CC)
                {
                    buffer.RemoveAt(0);
                    continue;
                }

                uint declaredSize = BitConverter.ToUInt32(buffer.ToArray(), 4);

                if (buffer.Count < declaredSize)
                    break;

                byte[] fullPacket = buffer.GetRange(0, (int)declaredSize).ToArray();

                InPacket packet = new(fullPacket);
                if (packet.IsValid)
                {
                    packets.Add(packet);
                }
                else
                {
                    // Invalid packet
                }

                buffer.RemoveRange(0, (int)declaredSize);
            }

            return packets;
        }
    }
}