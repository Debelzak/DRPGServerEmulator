
namespace DRPGServer.Game.Data.Models
{
    public struct DigimonSpawnOption
    {
        public ushort DigimonID { get; set; }
        public ushort Level { get; set; }
        public int STR { get; set; }
        public int AGI { get; set; }
        public int CON { get; set; }
        public int INT { get; set; }
        public ushort AppearanceRate { get; set; }
        public long ExpReward { get; set; }
        public double BitReward { get; set; }
        public uint DropGroup { get; set; }
    }
}
