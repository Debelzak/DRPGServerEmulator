using DRPGServer.Common;

namespace DRPGServer.Game.Entities
{
    public class DigimonSpawn(ushort digimonId)
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
        }

        public byte MapID { get; set; }
        public ushort DisplayDigimonID { get; set; } = digimonId;
        public short PosXMin { get; set; }
        public short PosXMax { get; set; }
        public short PosYMin { get; set; }
        public short PosYMax { get; set; }
        public short MaxCount { get; set; }
        public short RespawnTime { get; set; }

        readonly List<DigimonSpawnOption> _digimonPool = [];
        public IReadOnlyList<DigimonSpawnOption> DigimonPool => _digimonPool;

        public void AddSpawnOption(ushort digimonId, ushort level, int str, int agi, int con, int _int, long expReward, double bitReward, ushort appearanceRate)
        {
            DigimonSpawnOption option = new DigimonSpawnOption()
            {
                DigimonID = digimonId,
                Level = level,
                STR = str,
                AGI = agi,
                CON = con,
                INT = _int,
                AppearanceRate = appearanceRate,
                ExpReward = expReward,
                BitReward = bitReward,
            };
            _digimonPool.Add(option);
        }
    }
}