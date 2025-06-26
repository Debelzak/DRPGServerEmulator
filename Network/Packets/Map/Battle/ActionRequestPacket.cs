/* 0x01, 0x0A,
0x00, 0x00, 0xCF, 0xE1, 0x58, 0xF0, 0x59, 0x0D,   0xAC, 0x41, 0xB4, 0x55, 0x76, 0xB6, 0x4B, 0x99,
0xEC, 0x35, 0xF6, 0x92, 0xE8, 0x25, 0x4E, 0x2D,   0x14, 0x47, 0xB3, 0xB6, 0x8D, 0x35, 0xCE, 0x93,
0xB1, 0xE1,  */

using DRPGServer.Game.Entities;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class ActionRequestPacket(Digimon requester, Digimon? target = null) : OutPacket((ushort)PACKET_ID.MAP_BATTLE_ACTION_REQ)
    {
        public byte Action { get; set; } = 1; // 1 attack?, 2=run ,3 = reset action bar, 5 = cancel digimon summon,
        protected override void Serialize()
        {
            WriteByte(Action);
            WriteUShort(0x0a); // Icon. Attack = 10, Run = 4
            WriteByte(0); //??
            WriteBytes(requester.Serial.Data);
            WriteBytes((target != null) ? target.Serial.Data : new byte[16]);
        }
    }
}