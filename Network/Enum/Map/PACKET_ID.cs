namespace DRPGServer.Network.Enum.Map
{
    public enum PACKET_ID : ushort
    {
        UNKNOWN_PACKET = 0x00,
        MAP_JOIN_CHARACTER_REQ = 0x01,
        MAP_CHARACTER_DATA = 0x02,   //Needed in order to load map
        MAP_INVENTORY_DATA = 0x03,  //Needed in order to load map
        MAP_REFRESH_CHARACTER_INFO_REQ = 0x04, // (Client) Sents this soon after global connection, just like 0x0e packet, but needs to be replied with a packet 0x10 with user and digimon info.
        _0x05_PACKET = 0x05,
        MAP_0x07 = 0x07,
        _0x0e_PACKET = 0x0e,
        MAP_REFRESH_CHARACTER_INFO = 0x10, // (Server) Response to 0x04.
        MAP_FIELD_DIGIMON_UPDATE = 0x11,
        MAP_MOVE_CHARACTER_REQ = 0x20,
        MAP_CHAT_MESSAGE_NORMAL = 0x21,
        MAP_EXIT_GAME = 0x2b,
        MAP_MOVE_CONFIRM = 0xc4,
        MAP_JOIN_HANDSHAKE_RES = 0x33,
        MAP_JOIN_HANDSHAKE_REQ = 0x49,

        // Battle
        MAP_BATTLE_START_REQ = 0x61,
        MAP_BATTLE_START_RES = 0x62,
        MAP_BATTLE_CREATE = 0x63,
        MAP_BATTLE_BEGIN = 0x69,
        MAP_BATTLE_ATTACK_READY = 0x6b,

        //
        _0x73_PACKET = 0x73, // ?
        _0x74_PACKET = 0x74, // ?
        _0x7e_PACKET = 0x7e,  // ?
        _0x7f_PACKET = 0x7f,  // ?
        MAP_TELEPORT_CHARACTER = 0xb0,
        _0xb4_PACKET = 0xb4, // ?
        _0xe6_PACKET = 0xe6, // ?
        _0xf0_PACKET = 0xf0, // ?
        _0x121_PACKET = 0x121, // ?
        _0x152_PACKET = 0x152, // ?
    }
}
