using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatSession
    {
        public Dictionary<string, string> Fighters;

        public List<CombatRound> CombatRounds { get; set; }

        public CombatSession (string fighter1Id, string fighter2Id)
        {
            //List<CombatMove> initialerMoves = new List<CombatMove>();
            //initialerMoves.Add(new CombatMove() { FighterId = fighter1Id, Action = CombatActions.UNASSIGNED });
            //initialerMoves.Add(new CombatMove() { FighterId = fighter2Id, Action = CombatActions.UNASSIGNED });

            //CombatRound combatRound = new CombatRound(initialerMoves);
            //CombatRounds.Add(combatRound);
            Fighters[fighter1Id] = "White";
            Fighters[fighter2Id] = "Black";
        }

        public CombatResult AddMove(CombatMove NewMove) 
        {
            CombatResult combatResult = new CombatResult();


            return combatResult;
        }

    }
}
