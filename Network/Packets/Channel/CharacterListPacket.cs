using DRPGServer.Game.Entities;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterListPacket(IReadOnlyList<Character> characters) : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_LIST)
    {
        protected override void Serialize()
        {
            WriteInt(1); // Unknown int
            for (byte i = 0; i < 4; i++)
            {
                WriteCharacter(i, characters[i]);
            }
        }

        private void WriteCharacter(byte slot, Character character)
        {
            int offset = new Random().Next(0, 255);

            // Tamer info (124 bytes)
            WriteUInt(character.UID);
            WriteByte(character.TamerID);
            WriteString(character.Nickname, 21);
            WriteUShort(character.Level);
            WriteShort(character.PositionX);
            WriteShort(character.PositionY);
            WriteBytes([ // 44 bytes
                0x00, 0x00, 0x00, 0x00, // ??
                0x00, 0x00, 0x00, 0x00, // feet?
                0x00, 0x00, 0x00, 0x00, // legs?
                0x00, 0x00, 0x00, 0x00, // hands
                0x00, 0x00, 0x00, 0x00, // chest
                0x00, 0x00, 0x00, 0x00, // chest2
                0x00, 0x00, 0x00, 0x00, // head
                0x00, 0x00, 0x00, 0x00, // ??
                0x00, 0x00, 0x00, 0x00, // ??
                0x00, 0x00, 0x00, 0x00, // ??
                0x00, 0x00, 0x00, 0x00, // ??
            ]);
            WriteUShort(character.MainDigimon.DigimonID);
            WriteUShort(character.MainDigimon.Level);
            WriteString(character.MainDigimon.Name, 24);
            WriteInt(character.TotalBattles);
            WriteInt(character.TotalWins);
            WriteByte(slot);
            WriteByte(character.LocationID);
            WriteShort(0x0000);                                            //Unknown short // Padding?
            WriteInt(offset + character.LocationID + character.Level);     //Verification Calc? (Must be random offset below + locationId + tamer level)
            WriteInt(offset);                                              //Verification Offset? (Random offset, maybe between 1 and 255)
        }
    }
}