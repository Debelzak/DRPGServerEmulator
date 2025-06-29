namespace DRPGServer.Network.Enum
{
    public enum SERVER_TYPE : byte
    {
        UNKNOWN = 0,
        LOGIN_SERVER = 1,
        CHANNEL_SERVER = 2,
        MAP_SERVER = 3,
        GLOBAL_SERVER = 4
    }
}