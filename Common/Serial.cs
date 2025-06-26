
using System.Security.Cryptography;

namespace DRPGServer.Common
{
    public struct Serial
    {
        public byte[] Data { get; private set; } = new byte[16];

        public Serial()
        {
            RandomNumberGenerator.Fill(Data);
        }

        public Serial(byte[] data) 
        {
            Data = data;
        }

        public override readonly string ToString() => Convert.ToHexString(Data).ToUpperInvariant();
    }
}
