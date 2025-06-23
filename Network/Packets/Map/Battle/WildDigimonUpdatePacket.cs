using System.Security.Cryptography;
using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map.Battle
{
    public class WildDigimonUpdatePacket : OutPacket
    {
        public byte[] SpawnSerial { get; set; } = new byte[16];
        public ushort DigimonID { get; set; }
        public ushort Unk1 { get; set; } = 1;
        public string Name { get; set; } = string.Empty;
        public ushort Level { get; set; }
        public byte Classification { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public byte State { get; set; } = 1;
        public ushort FacingDirection { get; set; }

        public WildDigimonUpdatePacket(WildDigimon digimon) : base((ushort)PACKET_ID.MAP_FIELD_DIGIMON_UPDATE)
        {
            SpawnSerial = digimon.Serial;
            DigimonID = digimon.Leader.DigimonID;
            Name = digimon.Leader.Name;
            Level = digimon.Leader.Level;
            Classification = 2;
            PositionX = digimon.PositionX;
            PositionY = digimon.PositionY;
            FacingDirection = digimon.FacingDirection;
        }
        
        protected override void Serialize()
        {
            WriteBytes(SpawnSerial); //Field Spawn Hash ?? (For battle is different)
            WriteString(Name, 22); // name display
            WriteUShort(DigimonID); // DigimonID
            WriteUShort(Unk1); // Evolution Type?
            WriteUShort(Level); // Level
            WriteByte(Classification); // Name Icon (D) (B*) (V**) (E***) (L****) (Golden Crown) (Platinum Crown)
            WriteByte(State); // State byte (1 Spawn, 8 = Death, 11 = In-battle)
            WriteUShort(FacingDirection);
            WriteShort(PositionX); //PositionX
            WriteShort(PositionY); //PositionY
            WriteShort(PositionX); // Something to do with movement. official seems like it is always equals to PositionX
            WriteShort(PositionY); // Something to do with movement. official seems like it is always equals to PositionY
        }
    }
}