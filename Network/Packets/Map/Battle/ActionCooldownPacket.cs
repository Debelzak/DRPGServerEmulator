using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class ActionCooldownPacket(Game.Entities.Digimon digimon) : OutPacket((ushort)PACKET_ID.MAP_BATTLE_ACTION_COOLDOWN)
    {
        protected override void Serialize()
        {
            WriteBytes(digimon.Serial.Data);
            WriteInt(digimon.CurrentActionGauge);
            WriteInt(0); // ?
        }
    }
}