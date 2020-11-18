using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatVictory
    {
       // private string _victorFighterId = "";
        //private CombatVictoryConditions _condition = CombatVictoryConditions.VICTORY_KILL;


        //public CombatVictory(string victorFighterId, int condition) 
        //{
        //    VictorFighterId = victorFighterId;
        //    Condition = condition;
        //}
        public string VictorFighterId { get; set; }
        public int Condition { get; set; }
    }
}
