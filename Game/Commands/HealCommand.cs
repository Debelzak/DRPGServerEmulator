using DRPGServer.Game.Enum;
using DRPGServer.Network;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game.Commands
{
    class HealCommand : ICommand
    {
        public string Name => "heal";
        public string Description => "Fully restores current digimon. (Usage: /heal)";
        public byte AuthorityNeeded => (byte)AUTHORITY_ID.GAME_MASTER;

        public void Execute(Client client, string[] args)
        {
            var player = client.Player;
            if (player == null) return;

            player.Character.MainDigimon.Heal(player.Character.MainDigimon.MaxHP, player.Character.MainDigimon.MaxVP, player.Character.MainDigimon.MaxEVP);

            var updateDigimon = new SingleDigimonInfoPacket(player.Character.MainDigimon);
            player.Client.Send(updateDigimon);
        }
    }
}