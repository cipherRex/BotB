using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotB.Shared.CombatManagement.CombatHistoryResolvers;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public interface ICombatInstanceResolver
    {
        CombatResult Resolve(List<CombatMove> Moves);
    }

    public abstract class CombatInstanceResolverBase: ICombatInstanceResolver
    {
        const int MAX_POINTS = 10;

        protected CombatSession _combatSession;

        public CombatInstanceResolverBase(CombatSession Session) 
        {
            _combatSession = Session;
        }

        protected abstract CombatResult resolve(List<CombatMove> Moves);


        //public virtual CombatResult Resolve(CombatMove OpponentMove)
        public CombatResult Resolve(List<CombatMove> Moves)
        {

           

            //string opponentFighterId = OpponentMove.FighterId;
            //string thisFighterId = CombatHelpers.otherFighterId(opponentFighterId, _combatSession);

            string thisFighterId = Moves[0].FighterId;
            string opponentFighterId = Moves[1].FighterId;


            CombatResult combatResult = resolve( Moves);
            if (combatResult.MoveRestrictions == null) combatResult.MoveRestrictions = new List<KeyValuePair<string, CombatActions>>();
            if (combatResult.ShieldRecoil == null) combatResult.ShieldRecoil = new List<string>();
            if(combatResult.ShieldTaunt == null) combatResult.ShieldTaunt = new List<string>();


            //combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.BLOCK));

            if (combatResult.HPAdjustments.ContainsKey(thisFighterId) && combatResult.HPAdjustments[thisFighterId] > 0)
            {
                combatResult.HPAdjustments[thisFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustments[thisFighterId],
                                        combatResult.TotalRunningHPs[thisFighterId],
                                        MAX_POINTS);
            }
            if (combatResult.HPAdjustments.ContainsKey(opponentFighterId) && combatResult.HPAdjustments[opponentFighterId] > 0)
            {
                combatResult.HPAdjustments[opponentFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustments[opponentFighterId],
                                        combatResult.TotalRunningHPs[opponentFighterId],
                                        MAX_POINTS);
            }


            if (combatResult.TotalRunningHPs[thisFighterId] > MAX_POINTS) 
            {
                combatResult.TotalRunningHPs[thisFighterId] = MAX_POINTS;
            }

            if (combatResult.TotalRunningHPs[opponentFighterId] > MAX_POINTS)
            {
                combatResult.TotalRunningHPs[opponentFighterId] = MAX_POINTS;
            }


            if (combatResult.TotalRunningHPs[thisFighterId] < 1) 
            {
                CombatVictory combatVictory = new CombatVictory();
                combatVictory.Condition = 0;
                combatVictory.VictorFighterId = opponentFighterId;
                combatResult.Victory = combatVictory;
                //combatResult.Victory = new CombatVictory(opponentFighterId, (int)CombatVictoryConditions.VICTORY_KILL);

            }
            if (combatResult.TotalRunningHPs[opponentFighterId] < 1)
            {
                CombatVictory combatVictory = new CombatVictory();
                combatVictory.Condition = 0;
                combatVictory.VictorFighterId = thisFighterId;
                combatResult.Victory = combatVictory;
                //combatResult.Victory = new CombatVictory(thisFighterId, (int)CombatVictoryConditions.VICTORY_KILL);
            }


            if (combatResult.TotalRunningHPs[thisFighterId] < 1)
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.REST));
            }

            if (combatResult.TotalRunningHPs[opponentFighterId] < 1)
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(opponentFighterId, CombatActions.REST));
            }


            if (combatResult.TotalRunningHPs[thisFighterId] == MAX_POINTS)
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.REST));
            }

            if (combatResult.TotalRunningHPs[opponentFighterId] == MAX_POINTS)
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(opponentFighterId, CombatActions.REST));
            }

            return combatResult;
        }

        private int trimExecHpAdjustment(int adjustment, int currentTotal, int maxPoints)
        {
            if (adjustment + currentTotal <= maxPoints) 
            {
                return adjustment;
            }
            else 
            {
                return adjustment - ((currentTotal + adjustment) - maxPoints);
            }
        }

        /// <summary>
        /// Pass in a figher id and this cans the session to find the other
        /// fighter's id
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <returns></returns>
        //private string otherFighterId(string thisFighterId)
        //{ 
        //    return _combatSession.CombatRounds[0].Moves
        //            .Where(x => x.FighterId != thisFighterId)
        //            .FirstOrDefault().FighterId;
        //}


        /// <summary>
        /// Return the Hit Point total for fighter by calculating damage history
        /// </summary>
        /// <param name="fighterId"></param>
        /// <returns></returns>
        protected int totalHPs(string fighterId) 
        {


            return MAX_POINTS +
                    _combatSession.CombatRounds.Where(x => x.Result != null)
                    .SelectMany(x => x.Result.HPAdjustments)
                    .Where(x => x.Key == fighterId)
                    .Select(x => x.Value)
                    .Sum();

            //int ret = 0;

            //foreach(CombatRound round in _combatSession.CombatRounds) 
            //{ 
            //    if (round.Result != null) 
            //    {
            //        CombatResult combatResult = round.Result;
            //        foreach(KeyValuePair<string ,int> adjustment in combatResult.HPAdjustments) 
            //        {
            //            if (adjustment.Key == fighterId) 
            //            {
            //                if (adjustment.Key == fighterId)
            //                    ret = ret + adjustment.Value;
            //            }
            //        }
            //    }
            
            //}



            // return MAX_POINTS + ret;

        }



        ///// <summary>
        ///// Gives the sequence count for how many times This Fighter has successfully scored
        ///// against Opponent Fighter (counterparry does not count as a score)
        ///// </summary>
        ///// <param name="thisFighterId"></param>
        ///// <param name="opponentFighterId"></param>
        ///// <returns></returns>
        ////protected int numberPreviousSuccessfulStrikes(string thisFighterId, string opponentFighterId)
        //protected int numberPreviousSuccessfulStrikes(string thisFighterId)
        //{
        //    int ret = 0;

        //    string opponentFighterId = otherFighterId(thisFighterId);

        //    if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }

        //    for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
        //    {
        //        CombatRound combatRound = _combatSession.CombatRounds[i];

        //        if (combatRound.Result != null)
        //        {
        //            if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.SWING &&
        //                combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action == CombatEnums.REST
        //            )
        //            {
        //                ret++;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return ret;
        //}

        ///// <summary>
        ///// Gives the sequence count for how many times This Fighter has successfully healed
        ///// </summary>
        ///// <param name="thisFighterId"></param>
        ///// <param name="opponentFighterId"></param>
        ///// <returns></returns>
        ////protected int numberPreviousSuccessfulHeals(string thisFighterId, string opponentFighterId)
        //protected int numberPreviousSuccessfulHeals(string thisFighterId)
        //{
        //    int ret = 0;

        //    string opponentFighterId = otherFighterId(thisFighterId);

        //    if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


        //    for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
        //    {
        //        CombatRound combatRound = _combatSession.CombatRounds[i];

        //        if (combatRound.Result != null)
        //        {
        //            if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.REST &&
        //                combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action != CombatEnums.SWING
        //            )
        //            {
        //                ret++;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return ret;
        //}

        ///// <summary>
        ///// Gives the sequence count for how many times This Fighter has successfully Blocked
        ///// against Opponent Fighter (blocking a heal does not count)
        ///// </summary>
        ///// <param name="thisFighterId"></param>
        ///// <param name="opponentFighterId"></param>
        ///// <returns></returns>
        ////protected int numberPreviousSuccessfulBlocks(string thisFighterId, string opponentFighterId)
        //protected int numberPreviousSuccessfulBlocks(string thisFighterId)
        //{
        //    int ret = 0;
        //    string opponentFighterId = otherFighterId(thisFighterId);

        //    if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


        //    for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
        //    {
        //        CombatRound combatRound = _combatSession.CombatRounds[i];

        //        if (combatRound.Result != null)
        //        {
        //            if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
        //                combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action == CombatEnums.SWING
        //            )
        //            {
        //                ret++;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return ret;
        //}

        ///// <summary>
        ///// Gives the sequence count for how many times This Fighter has successfully Blocked
        ///// against Opponent Fighter (blocking a heal does not count)
        ///// </summary>
        ///// <param name="thisFighterId"></param>
        ///// <param name="opponentFighterId"></param>
        ///// <returns></returns>
        ////protected int numberPreviousFalseBlocks(string thisFighterId, string opponentFighterId)
        //protected int numberPreviousFalseBlocks(string thisFighterId)
        //{
        //    int ret = 0;
        //    string opponentFighterId = otherFighterId(thisFighterId);

        //    if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


        //    for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
        //    {
        //        CombatRound combatRound = _combatSession.CombatRounds[i];

        //        if (combatRound.Result != null)
        //        {
        //            if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
        //                combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action != CombatEnums.SWING
        //            )
        //            {
        //                ret++;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return ret;
        //}



    }
}
