using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Network.Handlers.Map.NPC
{
    class Patamon
    {
        public static void Handle(Client client, uint choiceId) {
            var packet = new NPCChoicePacket(choiceId);
            client.Send(packet);
        }
    }
}