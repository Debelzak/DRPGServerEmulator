using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterCreate : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_CREATE;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            if (client.User == null) return;

            // Reads data first
            int unknown1 = packet.ReadInt();
            int unknown2 = packet.ReadInt();
            ushort unknown3 = packet.ReadUShort();
            ushort seed1 = packet.ReadUShort();
            ushort seed2 = packet.ReadUShort();
            byte tamerId = packet.ReadByte();
            string characterName = packet.ReadString(21);
            ushort digimonId = packet.ReadUShort();
            string digimonName = packet.ReadString(26);

            byte targetSlot = 1;
            foreach (Character character in client.User.Characters)
            {
                if (character.UID == 0) break;
                targetSlot++;
            }
            // No available slots
            if (targetSlot > 4) return;

            // Creates new character
            var digimon = new Digimon(digimonId)
            {
                Name = digimonName,
                STR = 10,
                AGI = 10,
                CON = 10,
                INT = 10,
            };

            var newCharacter = new Character(digimon)
            {
                UID = (uint)new Random().Next(100000, 200000), // Database UID
                TamerID = tamerId,
                Nickname = characterName,
                Level = 1,
                PositionX = 56,
                PositionY = 56,
                EquippedItems = [],
                TotalBattles = 0,
                TotalWins = 0,
                LocationID = (byte)MAP_ID.VILLAGE_OF_BEGINNING,
            };
            client.User.SetCharacterSlot(targetSlot, newCharacter);

            // Send character creation result
            var characterCreate = new CharacterCreatePacket()
            {
                CharacterModel = newCharacter.TamerID,
                Nickname = newCharacter.Nickname,
                DigimonId = newCharacter.MainDigimon.DigimonID,
                DigimonNickname = newCharacter.MainDigimon.Name,
            };
            client.Send(characterCreate);

            //
            // Must reload user data here
            //

            // Re-sends character list packet with new character
            var refreshedCharactersPacket = new CharacterListPacket(client.User.Characters);
            client.Send(refreshedCharactersPacket);
        }
    }
}