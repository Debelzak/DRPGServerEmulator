using DRPGServer.Common;
using DRPGServer.Game.Data.Models;

namespace DRPGServer.Game.Entities
{
    public class DigimonSpawn()
    {
        public byte MapID { get; set; }
        public short PosXMin { get; set; }
        public short PosXMax { get; set; }
        public short PosYMin { get; set; }
        public short PosYMax { get; set; }
        public short MaxCount { get; set; }
        public short RespawnTime { get; set; }

        readonly List<DigimonSpawnOption> _digimonPool = [];
        public IReadOnlyList<DigimonSpawnOption> DigimonPool => _digimonPool;

        public void AddSpawnOption(ushort digimonId, ushort level, int str, int agi, int con, int _int, long expReward, double bitReward, ushort appearanceRate, uint dropGroup)
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
                DropGroup = dropGroup,
            };
            _digimonPool.Add(option);
        }
    }
}