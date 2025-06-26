using DRPGServer.Network.Enum;
using DRPGServer.Network.Enum.Map;

namespace DRPGServer.Network.Packets.Map
{
    public class TeleportCharacterPacket() : OutPacket((ushort)PACKET_ID.MAP_TELEPORT_CHARACTER)
    {
        public int MapID { get; set; }
        public short PosX { get; set; }
        public short PosY { get; set; }
        protected override void Serialize()
        {
            WriteInt(MapID);
            WriteShort(PosX);
            WriteShort(PosY);
        }
    }
}