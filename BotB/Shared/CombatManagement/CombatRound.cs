using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatRound
    {
        public List< CombatMove> Moves { get; set; }
        public CombatResult Result { get; set; }

    }
}
