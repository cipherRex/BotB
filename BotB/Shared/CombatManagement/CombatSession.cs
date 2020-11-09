using BotB.Shared.CombatManagement.CombatInstanceResolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatSession
    {
        public Dictionary<string, Fighter> Fighters;
        Dictionary<string, bool> _animationSemaphore = new Dictionary<string, bool>();
        
        public List<CombatRound> CombatRounds { get; set; }

        //public CombatSession (string fighter1Id, string fighter2Id)
        public CombatSession(Fighter Fighter1, Fighter Fighter2)
        {
            //List<CombatMove> initialerMoves = new List<CombatMove>();
            //initialerMoves.Add(new CombatMove() { FighterId = fighter1Id, Action = CombatActions.UNASSIGNED });
            //initialerMoves.Add(new CombatMove() { FighterId = fighter2Id, Action = CombatActions.UNASSIGNED });

            CombatRounds = new List<CombatRound>();

            //CombatRound combatRound = new CombatRound(initialerMoves);
            //CombatRounds.Add(combatRound);
            Fighters = new Dictionary<string, Fighter>();
            Fighter1.Color = "White";
            Fighters[Fighter1.id] = Fighter1;
            Fighter2.Color = "Black";
            Fighters[Fighter2.id] = Fighter2;
        }

        public CombatResult AddMove(CombatMove NewMove) 
        {



            if (CombatRounds.Count == 0 || CombatRounds[CombatRounds.Count - 1].Moves.Count == 0) 
            {
                CombatRound newCombatRound = new CombatRound(NewMove);
                CombatRounds.Add(newCombatRound);
                return null;
                //add the move
            } else 
            {

                CombatRound thisRound = CombatRounds[CombatRounds.Count - 1];

                thisRound.Moves.Add(NewMove);

                CombatResolverFactory combatResolverFactory = new CombatResolverFactory(this);
                ICombatInstanceResolver combatInstanceResolver =
                    combatResolverFactory.GetCombatResolver(CombatRounds[CombatRounds.Count - 1].Moves);

                CombatResult combatResult = combatInstanceResolver.Resolve(CombatRounds[CombatRounds.Count - 1].Moves[1]);
                thisRound.Result = combatResult;
                return combatResult;

                //add the move and get a CombatResult
            }

     
        }


        public Dictionary<string, bool> AnimationSemaphore
        {
            get
            {
                return _animationSemaphore;
            }
        }


    }
}
