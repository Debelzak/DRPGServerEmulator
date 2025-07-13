using DRPGServer.Game.Entities;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;

namespace DRPGServer.Network.Packets.Channel
{
    class CharacterListUserCheckPacket(Account account) : OutPacket((ushort)PACKET_ID.CHANNEL_USER_CHECK)
    {
        protected override void Serialize()
        {
            WriteString(account.Username, 22);
        }
    }
}