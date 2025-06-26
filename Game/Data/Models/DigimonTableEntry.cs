namespace DRPGServer.Game.Data.Models
{
    public struct DigimonTableEntry
    {
        public ushort DigimonID;
        public string Name;
        public int BaseHP;
        public int BaseVP;
        public int BaseATK;
        public int BaseDEF;
        public int BaseBR;
        public int ActionGauge;
        public byte Classification;
    }
}