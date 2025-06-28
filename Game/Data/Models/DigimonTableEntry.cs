namespace DRPGServer.Game.Data.Models
{
    public struct DigimonTableEntry
    {
        public ushort DigimonID;
        public string Name;
        public int BaseSTR;
        public int BaseAGI;
        public int BaseCON;
        public int BaseINT;
        public int ActionGauge;
        public byte Classification;
    }
}