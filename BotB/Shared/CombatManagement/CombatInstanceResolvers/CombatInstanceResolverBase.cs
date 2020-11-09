using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotB.Shared.CombatManagement.CombatHistoryResolvers;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public interface ICombatInstanceResolver
    {
        CombatResult Resolve(CombatMove OpponentMove);
    }

    public abstract class CombatInstanceResolverBase: ICombatInstanceResolver
    {
        protected CombatSession _combatSession;

        public CombatInstanceResolverBase(CombatSession Session) 
        {
            _combatSession = Session;
        }

        protected abstract CombatResult resolve(string ThisFighterId, string OpponentFighterId);


        //public virtual CombatResult Resolve(CombatMove OpponentMove)
        public CombatResult Resolve(CombatMove OpponentMove)
        {

            int MAXIMUM_HEALTH = 15;

            string opponentFighterId = OpponentMove.FighterId;
            //string thisFighterId = otherFighterId(opponentFighterId);
            string thisFighterId = CombatHelpers.otherFighterId(opponentFighterId, _combatSession);
            

            CombatResult combatResult = resolve(thisFighterId, opponentFighterId);


            if (combatResult.HPAdjustments.ContainsKey(thisFighterId))
            {
                combatResult.HPAdjustments[thisFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustments[thisFighterId],
                                        combatResult.TotalRunningHPs[thisFighterId],
                                        MAXIMUM_HEALTH);
            }
            if (combatResult.HPAdjustments.ContainsKey(opponentFighterId))
            {
                combatResult.HPAdjustments[opponentFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustments[opponentFighterId],
                                        combatResult.TotalRunningHPs[opponentFighterId],
                                        MAXIMUM_HEALTH);
            }


            if (combatResult.TotalRunningHPs[thisFighterId] < 1) 
            {
                combatResult.Victory = new CombatVictory(opponentFighterId, CombatVictoryConditions.VICTORY_KILL);
                
            }
            if (combatResult.TotalRunningHPs[opponentFighterId] < 1)
            {
                combatResult.Victory = new CombatVictory(thisFighterId, CombatVictoryConditions.VICTORY_KILL);
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

            var ret = _combatSession.CombatRounds.Where(x => x.Result != null)
                       .SelectMany(x => x.Result.HPAdjustments)
                       .Where(x => x.Key == fighterId)
                       .Select(x => x.Value)
                       .Sum();

            return ret;

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
