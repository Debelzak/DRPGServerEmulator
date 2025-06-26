using DRPGServer.Network.Enum;
using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum.Global;
using DRPGServer.Network.Packets.Global;

namespace DRPGServer.Network.Handlers.Global.UI
{
    class EncyclopediaHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.GLOBAL_ENCYCLOPEDIA_DATA;
        public SERVER_TYPE ServerType => SERVER_TYPE.GLOBAL_SERVER;
        public void Process(InPacket packet, Client client)
        {
            byte[] key_1 = packet.ReadBytes(16);
            byte[] key_2 = packet.ReadBytes(16);

            var encyclopediaData = new EncyclopediaDataPacket()
            {
                Key1 = key_1,
                Key2 = key_2,
            };
            
            client.Send(encyclopediaData);
        }
    }
}