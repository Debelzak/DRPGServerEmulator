using System.Security.Cryptography;
using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class WildDigimonUpdatePacket(WildDigimon digimon) : OutPacket((ushort)PACKET_ID.MAP_FIELD_DIGIMON_UPDATE)
    {
        protected override void Serialize()
        {
            WriteBytes(digimon.Serial.Data); //Field Spawn Hash ?? (For battle is different)
            WriteString(digimon.Digimons[0].Name, 21); // name display
            WriteByte(0);
            WriteUShort(digimon.DisplayDigimonID); // Display DigimonID
            WriteUShort(1); // Evolution Type?
            WriteUShort(digimon.Digimons[0].Level); // Level
            WriteByte(digimon.Digimons[0].Classification); // Name Icon (D) (B*) (V**) (E***) (L****) (Golden Crown) (Platinum Crown)
            WriteByte(digimon.IsBusy ? (byte)11 : digimon.IsDead ? (byte)6 : (byte)1); // State byte (1 Spawn, 6/8 = Death, 11 = In-battle)
            WriteUShort(digimon.FacingDirection);
            WriteShort(digimon.PositionX); //PositionX
            WriteShort(digimon.PositionY); //PositionY
            WriteShort(digimon.PositionX); // Something to do with movement. official seems like it is always equals to PositionX
            WriteShort(digimon.PositionY); // Something to do with movement. official seems like it is always equals to PositionY
        }
    }
}