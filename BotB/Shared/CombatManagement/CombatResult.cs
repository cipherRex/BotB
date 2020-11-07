using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatResult
    {
        public Dictionary<string, CombatAnimationInstruction> CombatAnimationInstructions { get; set; }
        public Dictionary<string, int> TotalRunningHPs { get; set; }
        public Dictionary<string ,int> HPAdjustment { get; set; }
        public CombatVictory Victory { get; set; }
        public string Comments { get; set; }
        public List<string> ShieldTaunt { get; set; }
        public List<string> ShieldRecoil { get; set; }
        public List<KeyValuePair<string, CombatEnums>> MoveRestrictions { get; set; }

        public CombatResult()
        {
            ShieldTaunt = new List<string>();
            ShieldRecoil = new List<string>();
            MoveRestrictions = new List<KeyValuePair<string, CombatEnums>>();
        }
    }

   
}
