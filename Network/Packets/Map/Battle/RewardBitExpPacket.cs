using DRPGServer.Game.Entities;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class RewardBitExpPacket(Digimon digimon, long expReward, double bitReward) : OutPacket((ushort)PACKET_ID.MAP_BATTLE_REWARD_BIT_EXP)
    {
        protected override void Serialize()
        {
            WriteBytes(digimon.Serial.Data);
            WriteLong(expReward);
            WriteDouble(bitReward);
        }
    }
}