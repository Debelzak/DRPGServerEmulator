using DRPGServer.Game.Data.DAOs;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Channel;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Packets.Channel;

namespace DRPGServer.Network.Handlers.Channel
{
    class CharacterDelete : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.CHANNEL_CHAR_DELETE;
        public SERVER_TYPE ServerType => SERVER_TYPE.CHANNEL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            if (client.User == null) return;
            if (client.Player != null) return;

            int unknown_1 = packet.ReadInt();
            int unknown_2 = packet.ReadInt();
            uint characterUid = packet.ReadUInt();
            string passwordMD5 = packet.ReadString(40);

            if (CharacterDAO.DeleteCharacter(characterUid, client.User.UID))
            {
                // Remove character from
                client.User.RemoveCharacter(characterUid);

                // Send response
                var deletePacket = new CharacterDeletePacket();
                client.Send(deletePacket);
                
                // Send chracter list again
                var refreshedCharacterList = new CharacterListPacket(client.User.Characters);
                client.Send(refreshedCharacterList);
            }
        }
    }
}