using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

/*
*   (Summary) Authentication packet, it is sent when the user clicks on a channel.
*   (Note 1) For some reason it sends the username, authkey and network adapter's mac when invalid
*            credentials is sent (just like client does requesting auth, seems like its just repeating         
*            the request packet along with the error)
*   (Note 2) When succeeded, it just responds with the username.
*/
namespace DRPGServer.Network.Packets.Channel
{
    class AuthenticationPacket() : OutPacket((ushort)PACKET_ID.CHANNEL_AUTH)
    {
        private int ResultCode = 3; // 2 = Success; 3 = Error ; 
        public ERROR_ID ErrorCode { get; set; } = ERROR_ID.INVALID_CREDENTIALS;
        public string Username { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
        public byte[] unknown_1 { get; private set; } = new byte[60];
        public string Mac1 { get; set; } = string.Empty;
        public string Mac2 { get; set; } = string.Empty;
        public string Mac3 { get; set; } = string.Empty;
        private byte[] unknown_2 = new byte[256];

        protected override void Serialize()
        {
            ResultCode = (ErrorCode == ERROR_ID.SUCCESS) ? 2 : 3;

            Write(ResultCode);
            Write((int)ErrorCode);
            Write(Username, 21);
            Write(AuthKey, 40);
            Write(unknown_1);
            Write(Mac1, 21);
            Write(Mac2, 21);
            Write(Mac3, 21);
            Write(unknown_2);
        }
    }
}