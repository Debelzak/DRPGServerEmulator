using System.Security.Cryptography;
using System.Text;

class Utils
{
    /// <summary>
    /// Converts a string to a fixed-size byte array, truncating if necessary.
    /// </summary>
    /// <param name="text">String to convert.</param>
    /// <param name="size">Fixed size of the output byte array.</param>
    /// <param name="encoding">Encoding to use (default is UTF8).</param>
    public static byte[] StringToFixedBytes(string text, int size, Encoding? encoding = null)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than 0.");

        encoding ??= Encoding.UTF8;
        int maxBytes = size; // agora pode usar todos os bytes

        Span<byte> buffer = stackalloc byte[512];
        int byteCount = encoding.GetBytes(text, buffer);

        if (byteCount <= maxBytes)
        {
            byte[] result = new byte[size];
            buffer[..byteCount].CopyTo(result);
            // Não colocar o zero no final
            return result;
        }

        // Busca binária
        int left = 0, right = text.Length;
        int bestFit = 0;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int len = encoding.GetByteCount(text.AsSpan(0, mid));

            if (len <= maxBytes)
            {
                bestFit = mid;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        byte[] output = new byte[size];
        encoding.GetBytes(text.AsSpan(0, bestFit), output.AsSpan());
        // Não colocar zero no final

        return output;
    }

    public static string MD5(string input)
    {
        using MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Converte os bytes para string hexadecimal
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }

    public static byte[] GenerateRandomSessionId(bool digi = false)
    {
        /*  byte[] id = new byte[16];
         RandomNumberGenerator.Fill(id);
         return id; */

        if (digi)
        {
            byte[] di = { 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, };
            return di;
        }

        byte[] id = { 0x35, 0x5B, 0x83, 0x6D, 0xFD, 0x18, 0x58, 0x11, 0xE8, 0x0C, 0x96, 0x46, 0xBC, 0x7D, 0xBF, 0x98 };
        return id;
    }
}