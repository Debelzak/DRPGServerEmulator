using DRPGServer.Game.Enum;
using DRPGServer.Network;
using DRPGServer.Network.Packets.Map.Character;

namespace DRPGServer.Game.Commands
{
    class TeleportCommand : ICommand
    {
        public string Name => "teleport";
        public string Description => "Teleports user to given position. (Usage: /teleport <x> <y> <location>)";
        public byte AuthorityNeeded => (byte)AUTHORITY_ID.GAME_MASTER;

        public void Execute(Client client, string[] args)
        {
            byte? currentLocation = client.Player?.MapID;
            if (currentLocation is null) return;

            if (args.Length < 2) return;

            if (!short.TryParse(args[0], out short newXPos) || !short.TryParse(args[1], out short newYPos))
                return;

            byte newLocId = (byte)currentLocation;

            if (args.Length >= 3 && !byte.TryParse(args[2], out newLocId))
                return;

            client.Player?.ChangeLocation(newLocId, newXPos, newYPos);
        }
    }
}