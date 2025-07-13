using DRPGServer.Game.Entities;
using DRPGServer.Managers;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map
{
    class MapJoinUserHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_JOIN_CHARACTER_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            string nickname = packet.ReadString(21);
            string username = packet.ReadString(21);
            byte unknown_1 = packet.ReadByte();

            // Verify with channelserver if authentication is ok, then proceed
            var user = ServerManager.MapServer.GetChannelUser(username);

            if (user == null) return;
            if (user.Username != username) return;

            var characterSlot = user.GetCharacterSlotByNickname(nickname);
            if (characterSlot == null) return;

            var character = user.GetCharacterSlot((byte)characterSlot);
            if (character == null) return;

            var zone = ZoneManager.GetZoneByMapID(character.MapID);
            if (zone == null) return;

            // Finally proceed
            var player = new Player(client, character, zone);
            client.SessionStart(user);
            player.Character.MainDigimon.SetOwner(player);
            client.SetPlayer(player);

            var userData = new CharacterDataPacket(player.Character);
            client.Send(userData);

            var inventoryData = new InventoryDataPacket();
            client.Send(inventoryData);

            // ??
            var _0x7e_29 = new _0x7e_Packet() { unknown_1 = 0x29 };
            client.Send(_0x7e_29);

            var _0x7e_2a = new _0x7e_Packet() { unknown_1 = 0x2a };
            client.Send(_0x7e_2a);

            var _0x7e_2b = new _0x7e_Packet() { unknown_1 = 0x2b };
            client.Send(_0x7e_2b);

            var _0x7e_2c = new _0x7e_Packet() { unknown_1 = 0x2c };
            client.Send(_0x7e_2c);

            var _0x7e_2d = new _0x7e_Packet() { unknown_1 = 0x2d };
            client.Send(_0x7e_2d);

            var _0x7e_2e = new _0x7e_Packet() { unknown_1 = 0x2e };
            client.Send(_0x7e_2e);

            var _0x7e_2f = new _0x7e_Packet() { unknown_1 = 0x2f };
            client.Send(_0x7e_2f);

            ///

            var _0x7f_3d = new _0x7f_Packet() { unknown_1 = 0x3d };
            client.Send(_0x7f_3d);

            var _0x7f_3e = new _0x7f_Packet() { unknown_1 = 0x3e };
            client.Send(_0x7f_3e);

            var _0x7f_3f = new _0x7f_Packet() { unknown_1 = 0x3f };
            client.Send(_0x7f_3f);

            var _0x7f_40 = new _0x7f_Packet() { unknown_1 = 0x40 };
            client.Send(_0x7f_40);

            var _0x7f_41 = new _0x7f_Packet() { unknown_1 = 0x41 };
            client.Send(_0x7f_41);

            var _0x7f_42 = new _0x7f_Packet() { unknown_1 = 0x42 };
            client.Send(_0x7f_42);

            var _0x7f_43 = new _0x7f_Packet() { unknown_1 = 0x43 };
            client.Send(_0x7f_43);
            // ??

            // ??
            var _0x05 = new _0x05_Packet();
            client.Send(_0x05);

            // ??
            var _0xe6 = new _0xe6_Packet();
            client.Send(_0xe6);

            // ??
            var _0x74 = new _0x74_Packet();
            client.Send(_0x74);

            // ??
            var _0x152 = new _0x152_Packet();
            client.Send(_0x152);

            // Digimon individual info
            var mainDigimonInfo = new SingleDigimonInfoPacket(player.Character.MainDigimon);
            client.Send(mainDigimonInfo);

            // ??
            var _0x73 = new _0x73_Packet();
            client.Send(_0x73);

        }
    }
}