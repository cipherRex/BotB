using BotB.Shared.CombatManagement.CombatInstanceResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BotB.Shared.CombatManagement
{
    public class CombatSession
    {
        public Dictionary<string, Fighter> Fighters;
        //Dictionary<string, bool> _animationSemaphore = new Dictionary<string, bool>();
        
        public List<CombatRound> CombatRounds { get; set; }

        //public CombatSession (string fighter1Id, string fighter2Id)
        public CombatSession(Fighter Fighter1, Fighter Fighter2)
        {
            //List<CombatMove> initialerMoves = new List<CombatMove>();
            //initialerMoves.Add(new CombatMove() { FighterId = fighter1Id, Action = CombatActions.UNASSIGNED });
            //initialerMoves.Add(new CombatMove() { FighterId = fighter2Id, Action = CombatActions.UNASSIGNED });
            AnimationSemaphore = new Dictionary<string, bool>();
            CombatRounds = new List<CombatRound>();

            //CombatRound combatRound = new CombatRound(initialerMoves);
            //CombatRounds.Add(combatRound);
            Fighters = new Dictionary<string, Fighter>();
            Fighter1.Color = "White";
            Fighters[Fighter1.id] = Fighter1;
            Fighter2.Color = "Black";
            Fighters[Fighter2.id] = Fighter2;
        }

        private readonly Mutex _moveMutex = new Mutex();
        public CombatResult AddMove(CombatMove NewMove) 
        {
            _moveMutex.WaitOne();
            try
            {
                if (CombatRounds.Count == 0 || CombatRounds[CombatRounds.Count - 1].Moves.Count % 2 == 0)
                {
                    CombatRound newCombatRound = new CombatRound(NewMove);
                    CombatRounds.Add(newCombatRound);
                    return null;
                    //add the move
                }
                else
                {

                    CombatRound thisRound = CombatRounds[CombatRounds.Count - 1];

                    thisRound.Moves.Add(NewMove);

                    CombatResolverFactory combatResolverFactory = new CombatResolverFactory(this);
                    ICombatInstanceResolver combatInstanceResolver =
                        combatResolverFactory.GetCombatResolver(CombatRounds[CombatRounds.Count - 1].Moves);

                    CombatResult combatResult = combatInstanceResolver.Resolve(CombatRounds[CombatRounds.Count - 1].Moves);
                    thisRound.Result = combatResult;
                    return combatResult;

                    //add the move and get a CombatResult
                }
            }
            finally { 
                _moveMutex.ReleaseMutex();
            }
        }

        public Dictionary<string, bool> AnimationSemaphore
        {
            get; set;
            //{
                //return _animationSemaphore;
            //}
        }


    }
}
