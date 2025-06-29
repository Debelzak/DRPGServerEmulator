namespace DRPGServer.Network.Enum.Global
{
    public enum PACKET_ID : ushort
    {
        UNKNOWN_PACKET = 0x00,
        GLOBAL_USER_AUTH_REQ = 0x64, // (Client) First packet sent by client
        _0x65_PACKET = 0x65,
        GLOBAL_CHARACTER_JOIN_REQ = 0xde,
        _0x0e_PACKET = 0x0e,    // (Server/Client) Packet like map server, first packet sent by server
        _0xe1_PACKET = 0xe1,    // (Server) Great packet server sends after join at GLOBAL_CHARACTER_JOIN_REQ (?), followed by _0x65_PACKET
        _0xcb_PACKET = 0xcb,    // (Client) Great packet client sends after join
        GLOBAL_ENCYCLOPEDIA_DATA = 0x7b,    // (Client/Server) This is sent soon after _0x65_PACKET, followed by  _0x7c_PACKET, then server responds
        GLOBAL_ENCYCLOPEDIA_COLLECTION = 0x7c,    // (Client/Server) Client sends first / Server returns
        _0xc4_PACKET = 0xc4,    // (Client/Server) Empty packet. Seems like a keep-alive handshake. Client sends time after time, server responds.
        _0xc8_PACKET = 0xc8,    // (Client) Blue (Private messsage) sent in chat message.
        _0xca_PACKET = 0xca,    // (Server) Green (Megaphone?) chat message.
        _0xcc_PACKET = 0xcc,    // (Server) player events like kill a boss

    }
}
