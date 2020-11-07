using System;
using System.Collections.Generic;
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
    }
}
