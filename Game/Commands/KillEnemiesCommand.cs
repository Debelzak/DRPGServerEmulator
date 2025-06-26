using DRPGServer.Game.Enum;
using DRPGServer.Network;

namespace DRPGServer.Game.Commands
{
    class KillEnemiesCommand : ICommand
    {
        public string Name => "killenemies";
        public string Description => "Kills enemy team in battle. (Usage: /killenemies)";
        public byte AuthorityNeeded => (byte)AUTHORITY_ID.GAME_MASTER;

        public void Execute(Client client, string[] args)
        {
            var battle = client.Player?.Battle;
            if (battle == null) return;

            battle.KillTeamB();
        }
    }
}