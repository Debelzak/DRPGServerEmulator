using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    class BattleEndPacket(byte ResultType) : OutPacket((ushort)PACKET_ID.MAP_BATTLE_END_PACKET)
    {
        protected override void Serialize()
        {
            WriteByte(ResultType);   // 1=Win; 2=Lose; 3=Battle cancel; Any else = Disconnect
        }
    }
}