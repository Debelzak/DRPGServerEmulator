namespace DRPGServer.Network.Enum.Map
{
    public enum PACKET_ID : ushort
    {
        UNKNOWN_PACKET = 0x00,
        MAP_JOIN_USER_REQ = 0x01,
        MAP_JOIN_USER_RES = 0x02,   //Needed in order to load map
        MAP_INVENTORY_DATA = 0x03,  //Needed in order to load map
        MAP_0x0007 = 0x07,
        _0x000e_PACKET = 0x0e,
        MAP_MOVE_CHARACTER_REQ = 0x20,
        MAP_CHAT_MESSAGE_NORMAL = 0x21,
        MAP_EXIT_GAME = 0x2b,
        MAP_MOVE_CONFIRM = 0xc4,
        MAP_JOIN_TICK_RES = 0x33,
        MAP_JOIN_TICK_REQ = 0x49,
        _0x007e_PACKET = 0x7e,  // Inventory load?
        _0x007f_PACKET = 0x7f,  // ?
    }
}
