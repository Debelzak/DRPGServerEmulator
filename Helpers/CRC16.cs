public static class CRC16
{
    static readonly ushort[] table;

    static CRC16()
    {
        table = new ushort[256];
        const ushort polynomial = 0x1021; // CRC16-XMODEM Poly

        for (ushort i = 0; i < table.Length; ++i)
        {
            ushort value = 0;
            ushort temp = (ushort)(i << 8);

            for (byte j = 0; j < 8; ++j)
            {
                if (((value ^ temp) & 0x8000) != 0)
                    value = (ushort)((value << 1) ^ polynomial);
                else
                    value <<= 1;

                temp <<= 1;
            }

            table[i] = value;
        }
    }

    public static ushort Compute(byte[] data, int padding = 14)
    {
        byte[] padded = new byte[data.Length + padding];
        Buffer.BlockCopy(data, 0, padded, 0, data.Length);

        ushort crc = 0x0000;
        foreach (byte b in padded)
        {
            byte index = (byte)((crc >> 8) ^ b);
            crc = (ushort)((crc << 8) ^ table[index]);
        }

        return crc;
    }
}
