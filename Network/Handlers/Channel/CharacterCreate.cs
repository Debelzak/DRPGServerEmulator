using DRPGServer.Game.Enum;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;
using static DRPGServer.Network.Packets.Channel.CharacterListPacket;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterCreate : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_CREATE;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
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

            // Send character creation result
            var characterCreate = new CharacterCreatePacket()
            {
                CharacterModel = tamerId,
                Nickname = characterName,
                DigimonId = digimonId,
                DigimonNickname = digimonName
            };
            client.Send(characterCreate);

            // Re-sends character list packet with new character
            var data2 = new CharacterListPacket
            {
                Slot2 = new Character
                {
                    id = 20000,
                    tamerId = tamerId,
                    nickname = characterName,
                    level = 1,
                    positionX = 57,
                    positionY = 86,
                    equippedItems = [],
                    digimonId = digimonId,
                    digimonLevel = 1,
                    digimonNickname = digimonName,
                    totalBattles = 0,
                    totalWins = 0,
                    slotId = 1,
                    locationId = (byte)MAP_ID.VILLAGE_OF_BEGINNING,
                }
            };
            client.Send(data2);
        }
    }
}