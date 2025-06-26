using DRPGServer.Common;

namespace DRPGServer.Game.Entities
{
    public class DigimonSpawn(ushort digimonId)
    {
        public struct DigimonSpawnPartner
        {
            public ushort DigimonID { get; set; }
            public ushort Level { get; set; }
            public ushort AppearanceRate { get; set; }
            public ushort Count { get; set; }
        }

        public Serial Serial { get; private set; } = new();
        public byte MapID { get; set; }
        public ushort DigimonID { get; set; } = digimonId;
        public ushort Level { get; set; }
        public short PosXMin { get; set; }
        public short PosXMax { get; set; }
        public short PosYMin { get; set; }
        public short PosYMax { get; set; }
        public short MaxCount { get; set; }
        public short RespawnTime { get; set; }

        readonly List<DigimonSpawnPartner> _partnerPool = [];
        public IReadOnlyList<DigimonSpawnPartner> PartnerPool => _partnerPool;

        public void AddPartnerOption(ushort digimonId, ushort level, ushort appearanceRate, ushort count)
        {
            DigimonSpawnPartner partner = new DigimonSpawnPartner() { DigimonID = digimonId, Level = level, Count = count, AppearanceRate = appearanceRate };
            _partnerPool.Add(partner);
        }
    }
}