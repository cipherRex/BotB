using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatRound
    {
        private List<CombatMove> _moves = null;

        public List< CombatMove> Moves { get; }
        public CombatResult Result { get; set; }


        public CombatRound(List<CombatMove> PlayerMoves)
        {
            _moves = PlayerMoves;
        }
    }

}
