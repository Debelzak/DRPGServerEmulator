using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class NPCChoicePacket(uint choiceId) : OutPacket((ushort)PACKET_ID.MAP_NPC_DIALOG_CHOICE_REQ)
    {
        protected override void Serialize()
        {
            WriteUInt(choiceId); // Just server echo confirmation if success
        }
    }
}