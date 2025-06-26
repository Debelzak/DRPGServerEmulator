using DRPGServer.Game.Entities;
using DRPGServer.Game.Enum;
using DRPGServer.Network;

namespace DRPGServer.Game.Commands
{
    class ReloadSpawnsCommand : ICommand
    {
        public string Name => "reloadspawns";
        public string Description => "Reloads all spawns in current map. (Usage: /reloadspawns)";
        public byte AuthorityNeeded => (byte)AUTHORITY_ID.ADMINISTRATOR;

        public void Execute(Client client, string[] args)
        {
            var player = client.Player;
            if (player == null) return;

            player.Zone.ReloadSpawns();
        }
    }
}