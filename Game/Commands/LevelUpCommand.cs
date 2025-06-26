using DRPGServer.Game.Enum;
using DRPGServer.Network;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game.Commands
{
    class LevelUpCommand : ICommand
    {
        public string Name => "levelup";
        public string Description => "Levels up current digimon. (Usage: /levelup <lv_amount>)";
        public byte AuthorityNeeded => (byte)AUTHORITY_ID.GAME_MASTER;

        public void Execute(Client client, string[] args)
        {
            var player = client.Player;
            if (player == null) return;

            player.Character.MainDigimon.LevelUp();

            var updateDigimon = new SingleDigimonInfoPacket(player.Character.MainDigimon);
            player.Client.Send(updateDigimon);
        }
    }
}