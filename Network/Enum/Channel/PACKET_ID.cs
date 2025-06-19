namespace DRPGServer.Network.Enum.Channel
{
    public enum PACKET_ID : ushort
    {
        UNKNOWN_PACKET = 0x00,
        CHANNEL_SELECT = 0x03,
        CHANNEL_AUTH = 0x04,
        CHANNEL_CHAR_CREATE = 0x10,
        CHANNEL_CHAR_DELETE = 0x11,
        CHANNEL_CHAR_LIST = 0x12,
        CHANNEL_USER_CHECK = 0x14,
    }
}
