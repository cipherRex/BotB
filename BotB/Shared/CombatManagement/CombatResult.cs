﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatResult
    {
        public Dictionary<string, CombatAnimationInstruction> CombatAnimationInstructions { get; set; }
        public Dictionary<string, int> TotalRunningHPs { get; set; }
        public Dictionary<string ,int> HPAdjustments { get; set; }
        public CombatVictory Victory { get; set; }
        public string Comments { get; set; }
        public List<string> ShieldTaunt { get; set; }
        public List<string> ShieldRecoil { get; set; }
        public List<KeyValuePair<string, CombatActions>> MoveRestrictions { get; set; }

        public CombatResult()
        {
            CombatAnimationInstructions = new Dictionary<string, CombatAnimationInstruction>();
            TotalRunningHPs = new Dictionary<string, int>();
            HPAdjustments = new Dictionary<string, int>();

            ShieldTaunt = new List<string>();
            ShieldRecoil = new List<string>();
            MoveRestrictions = new List<KeyValuePair<string, CombatActions>>();
        }
    }

   
}
