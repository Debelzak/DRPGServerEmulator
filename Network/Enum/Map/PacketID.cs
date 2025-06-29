namespace DRPGServer.Network.Enum.Map
{
    public enum PACKET_ID : ushort
    {
        UNKNOWN_PACKET = 0x0000,
        MAP_JOIN_CHARACTER_REQ = 0x0001,
        MAP_CHARACTER_DATA = 0x0002,   //Needed in order to load map
        MAP_INVENTORY_DATA = 0x0003,  //Needed in order to load map
        MAP_REFRESH_CHARACTER_INFO_REQ = 0x0004, // (Client) Sents this soon after global connection, just like 0x0e packet, but needs to be replied with a packet 0x10 with user and digimon info.
        _0x05_PACKET = 0x0005,
        MAP_0x07 = 0x0007,
        _0x0e_PACKET = 0x000e,
        MAP_REFRESH_CHARACTER_INFO = 0x0010, // (Server) Response to 0x04.
        MAP_FIELD_DIGIMON_UPDATE = 0x0011,
        MAP_MOVE_CHARACTER_REQ = 0x0020,
        MAP_CHAT_MESSAGE_NORMAL = 0x0021,
        MAP_EXIT_GAME = 0x002b,
        MAP_MOVE_CONFIRM = 0x00c4,
        MAP_JOIN_HANDSHAKE_RES = 0x0033,
        MAP_JOIN_HANDSHAKE_REQ = 0x0049,
        MAP_SINGLE_DIGIMON_INFO_REQ = 0x00b4,
        

        // Battle
        MAP_BATTLE_ACTION_REQ = 0x0035,
        MAP_BATTLE_START_REQ = 0x0061,
        MAP_BATTLE_START_RES = 0x0062,
        MAP_BATTLE_CREATE = 0x0063,
        MAP_BATTLE_65 = 0x0065, // Run packet?
        MAP_BATTLE_ACTION_RESULTS = 0x0067,
        MAP_BATTLE_LOCK_ACTIONBAR = 0x0068,
        MAP_BATTLE_BEGIN = 0x0069,
        MAP_BATTLE_6a = 0x006a, // run related?
        MAP_BATTLE_ACTION_COOLDOWN = 0x006b,
        MAP_BATTLE_END_PACKET = 0x006d,
        MAP_BATTLE_REWARD_BIT_EXP = 0x006e,
        MAP_BATTLE_LEVELUP = 0x0070,


        // UI
        MAP_DIGIMON_ADD_ABILITY_POINT_REQ = 0x0071,


        // NPCs
        MAP_NPC_DIALOG_CHOICE_REQ = 0x0056,

        // Inventory
        MAP_INVENTORY_ITEM_RECEIVE = 0x00c9,
        MAP_INVENTORY_REFRESH = 0x00fc,


        _0xb1_PACKET = 0x00b1, // ?
        MAP_REFRESH_DIGIMON_STATUS = 0x00b2, // ?
        _0x73_PACKET = 0x0073, // ?
        _0x74_PACKET = 0x0074, // ?
        _0x7e_PACKET = 0x007e,  // ?
        _0x7f_PACKET = 0x007f,  // ?
        MAP_TELEPORT_CHARACTER = 0x00b0,
        _0xe6_PACKET = 0x00e6, // ?
        _0xf0_PACKET = 0x00f0, // ?
        _0x121_PACKET = 0x0121, // ?
        _0x152_PACKET = 0x0152, // ?
    }
}
