using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatVictory
    {
        private string _victorFighterId = "";
        private CombatVictoryConditions _condition = CombatVictoryConditions.VICTORY_KILL;


        public CombatVictory(string victorFighterId, CombatVictoryConditions condition) 
        {
            _victorFighterId = victorFighterId;
            _condition = condition;
        }
        public string VictorFighterId { get { return _victorFighterId; } }
        public CombatVictoryConditions Condition { get { return _condition; } }
    }
}
