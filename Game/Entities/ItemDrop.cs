using DRPGServer.Common;

namespace DRPGServer.Game.Data.Models
{
    public class ItemDrop
    {
        private static uint NextUID = 10000; // starting point
        public uint UID { get; private set; } = GetNextUID();
        public uint ItemID;
        public short PosX;
        public short PosY;
        public long DropTime { get; private set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public Serial? OwnerSerial;

        private static uint GetNextUID()
        {
            return Interlocked.Increment(ref NextUID);
        }
    }
}