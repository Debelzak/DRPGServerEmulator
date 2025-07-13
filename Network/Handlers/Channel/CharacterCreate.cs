using System.Text.RegularExpressions;
using DRPGServer.Game.Data.Database.DAOs;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    partial class CharacterCreate : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_CREATE;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            if (client.User == null) return;

            // 4th digimon
            var fourth_digimon_config = ServerConsts.Get("CHARACTER_CREATE_4TH_DIGIMON");

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

            byte targetSlot = 0;
            foreach (Character character in client.User.Characters)
            {
                if (character.UID == 0) break;
                targetSlot++;
            }
            // No available slots
            if (targetSlot > 3)
                return;

            // Check if tamer is valid
            if (tamerId != 1 && tamerId != 2 && tamerId != 3 && tamerId != 4) return;

            // Check if tamer name is valid
            if (!Regex().IsMatch(characterName)) return;

            // Check if digimon is valid
            if (digimonId != 10 && digimonId != 11 && digimonId != 12 &&
                digimonId != 24 && digimonId != 27 && digimonId != 31 &&
                digimonId != 40
            )
                return;
            
            // Check if digimon name is valid
            if (!Regex().IsMatch(digimonName)) return;

            // Check if fourth digimon was chosen and if it is valid.
            if (digimonId == 24 && fourth_digimon_config != 0) return;
            if (digimonId == 27 && fourth_digimon_config != 1) return;
            if (digimonId == 31 && fourth_digimon_config != 2) return;
            if (digimonId == 40 && fourth_digimon_config != 3) return;

            // begin
            var characterCreatePacket = new CharacterCreatePacket()
            {
                ErrorCode = 0,
                CharacterModel = tamerId,
                Nickname = characterName,
                DigimonId = digimonId,
                DigimonNickname = digimonName,
            };

            if (!CharacterDAO.IsNicknameAvailable(characterName)) // tamer name already exists
            {
                characterCreatePacket.ErrorCode = 102;
                client.Send(characterCreatePacket);
                return;
            }

            var createdCharacterUid = CharacterDAO.CreateCharacter(client.User.UID, targetSlot, characterName, tamerId, digimonName, digimonId);
            if (createdCharacterUid == null) return;

            var createdCharacter = CharacterDAO.GetCharacterByUID((uint)createdCharacterUid);
            if (createdCharacter == null) return;

            client.Send(characterCreatePacket);

            client.User.SetCharacterSlot(targetSlot, createdCharacter);
            // Re-sends character list packet with new character
            var refreshedCharactersPacket = new CharacterListPacket(client.User.Characters);
            client.Send(refreshedCharactersPacket);
        }

        [GeneratedRegex(@"^[a-zA-Z0-9]+$")]
        private static partial Regex Regex();
    }
}