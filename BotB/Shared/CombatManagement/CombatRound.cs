using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatRound
    {
        private List<CombatMove> _moves = new List<CombatMove>();

        public List< CombatMove> Moves { get { return _moves; } }
        public CombatResult Result { get; set; }


        public CombatRound(CombatMove PlayerMove)
        {
            _moves.Add(PlayerMove);
        }


    }

}
