using DRPGServer.Common;
using DRPGServer.Game;
using DRPGServer.Game.Entities;

namespace DRGPServer.Managers
{
    public class BattleManager
    {
        private static readonly Dictionary<string, Battle> activeBattles = [];

        public static Battle? CreateBattle(Player participant, WildDigimon enemy)
        {
            
            enemy.IsBusy = true;
            
            var battle = new Battle(participant, enemy);
            activeBattles.Add(battle.Serial.ToString(), battle);
            return battle;
        }

        public static void DeleteBattle(Battle battle)
        {
            activeBattles.Remove(battle.Serial.ToString());
        }
    }

}