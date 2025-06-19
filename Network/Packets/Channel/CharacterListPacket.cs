using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterListPacket() : OutPacket((ushort)PACKET_ID.CHANNEL_CHAR_LIST)
    {
        public Character Slot1 { get; set; } = new Character(){ slotId = 0 };
        public Character Slot2 { get; set; } = new Character() { slotId = 1 };
        public Character Slot3 { get; set; } = new Character() { slotId = 2 };
        public Character Slot4 { get; set; } = new Character() { slotId = 3 };

        protected override void Serialize()
        {
            Write(1);              // Unknown int
            Slot1 = new Character
            {
                id = 50000,
                tamerId = 1,
                nickname = "Takato",
                level = 1,
                positionX = 57,
                positionY = 86,
                equippedItems = [],
                digimonId = 10,
                digimonLevel = 1,
                digimonNickname = "Guilmon",
                totalBattles = 0,
                totalWins = 0,
                slotId = 0,
                locationId = 1,
            };
            WriteCharacter(Slot1);
            WriteCharacter(Slot2);
            WriteCharacter(Slot3);
            WriteCharacter(Slot4);
        }

        private void WriteCharacter(Character character)
        {
            int offset = new Random().Next(0, 255);

            // Tamer info (124 bytes)
            Write(character.id);
            Write(character.tamerId);
            Write(character.nickname, 21);
            Write(character.level);
            Write(character.positionX);
            Write(character.positionY);
            Write([ // 44 bytes
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
            Write(character.digimonId);
            Write(character.digimonLevel);
            Write(character.digimonNickname, 24);
            Write(character.totalBattles);
            Write(character.totalWins);
            Write(character.slotId);
            Write(character.locationId);
            Write((short)0x0000);                                       //Unknown short // Padding?
            Write(offset + character.locationId + character.level);     //Verification Calc? (Must be random offset below + locationId + tamer level)
            Write(offset);                                              //Verification Offset? (Random offset, maybe between 1 and 255)
        }
        
        public struct Character
        {
            public uint id;
            public byte tamerId;
            public string nickname;
            public ushort level;
            public short positionX;
            public short positionY;
            public byte[] equippedItems;
            public ushort digimonId;
            public ushort digimonLevel;
            public string digimonNickname;
            public int totalBattles;
            public int totalWins;
            public byte slotId;
            public byte locationId;
        }
    }
}