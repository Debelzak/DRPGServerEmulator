using DRPGServer.Common;

namespace DRPGServer.Game.Entities
{
    public class Portal
    {
        public int MapID { get; set; }
        public short PosX { get; set; }
        public short PosY { get; set; }
        public int DestMapID { get; set; }
        public short DestPosX { get; set; }
        public short DestPosY { get; set; }

        public bool AffectsPosition(int playerX, int playerY)
        {
            int portalRadius = 2;
            
            int dx = playerX - PosX;
            int dy = playerY - PosY;

            int distanceSquared = dx * dx + dy * dy;
            int radiusSquared = portalRadius * portalRadius;

            return distanceSquared <= radiusSquared;
        }
    }
}