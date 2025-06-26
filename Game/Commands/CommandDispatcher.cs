using DRPGServer.Network;

namespace DRPGServer.Game.Commands
{
    public static class CommandDispatcher
    {
        private static readonly Dictionary<string, ICommand> _commands = new();

        static CommandDispatcher()
        {
            Register(new TeleportCommand());
            Register(new ReloadSpawnsCommand());
            Register(new KillEnemiesCommand());
            Register(new LevelUpCommand());
            Register(new HealCommand());            
        }

        public static void Register(ICommand command)
        {
            Logger.Info($"Registered command /{command.Name}.");
            _commands[command.Name.ToLower()] = command;
        }

        public static bool Execute(Client client, string input)
        {
            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return false;

            var cmdName = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (_commands.TryGetValue(cmdName, out var command))
            {
                if (client.User == null || client.User.AuthorityLevel < command.AuthorityNeeded)
                    return false;
                
                command.Execute(client, args);
                return true;
            }
            else
            {
                Logger.Warn($"Trying to dispatch unknown command: /{cmdName}");
                return false;
            }
        }
    }
}