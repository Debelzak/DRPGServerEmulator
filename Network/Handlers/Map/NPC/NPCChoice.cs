using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;
using DRPGServer.Network.Handlers.Map.NPC;
using DRPGServer.Network.Packets;

namespace DRPGServer.Network.Handlers.Map
{
    class NPCChoiceHandler : IPacketHandler
    {
        public ushort Opcode => (ushort)PACKET_ID.MAP_NPC_DIALOG_CHOICE_REQ;
        public SERVER_TYPE ServerType => SERVER_TYPE.MAP_SERVER;
        public void Process(InPacket packet, Client client)
        {
            uint choiceId = packet.ReadUInt();

            switch (choiceId)
            {
                case 1: Patamon.Handle(client, choiceId); break;
                case 2: Botamon.Handle(client, choiceId); break;
                case 3: Mochimon.Handle(client, choiceId); break;
                case 4: Pyocomon.Handle(client, choiceId); break;
                case 5: Tunomon.Handle(client, choiceId); break;
                case 7: Elecmon.Handle(client, choiceId); break;
                case 8: Elecmon.Handle(client, choiceId); break;
                default: return;
            }
        }
    }
}