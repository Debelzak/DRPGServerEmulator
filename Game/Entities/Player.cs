using DRPGServer.Managers;
using DRPGServer.Network;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game.Entities
{
    public class Player : IDisposable
    {
        public Client Client { get; private set; }
        public Character Character { get; private set; }
        public Zone Zone { get; set; }
        public Battle? Battle;
        public byte MapID => Character.LocationID;
        public short PositionX => Character.PositionX;
        public short PositionY => Character.PositionY;

        public bool CanMove = true;

        private bool disposed = false;

        public Player(Client client, Character character, Zone zone)
        {
            Client = client;
            Character = character;
            Zone = zone;
            Zone.AddPlayer(this);
        }

        public void ChangeLocation(byte mapId, short x, short y)
        {
            if (Zone.MapID != mapId)
            {
                if (!ZoneManager.TransferPlayer(this, mapId))
                    return;
            }

            Character.LocationID = mapId;
            Character.PositionX = x;
            Character.PositionY = y;

            var teleport = new TeleportCharacterPacket() { MapID = mapId, PosX = x, PosY = y };
            Client.Send(teleport);
        }

        public void ChangePosition(short x, short y)
        {
            Character.PositionX = x;
            Character.PositionY = y;
        }

        /// <summary>
        /// Removes player from zone and releases all resources.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;

            Battle?.Participants.Remove(this);
            Zone.RemovePlayer(this);

            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}