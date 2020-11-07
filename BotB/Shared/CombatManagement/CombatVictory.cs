using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatVictory
    {
        public string VictorFighterId { get; set; }
        public CombatVictoryCondition Condition { get; set; }
    }
}
