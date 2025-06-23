using DRPGServer.Network;

namespace DRPGServer.Game.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        byte AuthorityNeeded { get; }

        void Execute(Client client, string[] args);
    }
}