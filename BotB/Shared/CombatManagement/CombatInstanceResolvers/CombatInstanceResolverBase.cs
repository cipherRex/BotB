using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public abstract class CombatInstanceResolverBase: ICombatInstanceResolver
    {
        protected CombatSession _combatSession;

        public CombatInstanceResolverBase(CombatSession Session) 
        {
            _combatSession = Session;
        }

        public virtual CombatResult Resolve(CombatMove OpponentMove) 
        {

            int MAXIMUM_HEALTH = 15;

            string opponentFighterId = OpponentMove.FighterId;
            string thisFighterId = otherFighterId(opponentFighterId);

            CombatResult combatResult = resolve(thisFighterId, opponentFighterId);

            if (combatResult.HPAdjustment.ContainsKey(thisFighterId))
            {
                combatResult.HPAdjustment[thisFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustment[thisFighterId],
                                        combatResult.TotalRunningHPs[thisFighterId],
                                        MAXIMUM_HEALTH);
            }

            if (combatResult.HPAdjustment.ContainsKey(opponentFighterId))
            {
                combatResult.HPAdjustment[opponentFighterId] =
                    trimExecHpAdjustment(combatResult.HPAdjustment[opponentFighterId],
                                        combatResult.TotalRunningHPs[opponentFighterId],
                                        MAXIMUM_HEALTH);
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
        private string otherFighterId(string thisFighterId)
        { 
            return _combatSession.CombatRounds[0].Moves
                    .Where(x => x.FighterId != thisFighterId)
                    .FirstOrDefault().FighterId;
        }

        protected abstract CombatResult resolve(string ThisFighterId, string OpponentFighterId);

        /// <summary>
        /// Return the Hit Point total for fighter by calculating damage history
        /// </summary>
        /// <param name="fighterId"></param>
        /// <returns></returns>
        protected int totalHPs(string fighterId) 
        {

            var ret = _combatSession.CombatRounds.Where(x => x.Result != null)
                       .SelectMany(x => x.Result.HPAdjustment)
                       .Where(x => x.Key == fighterId)
                       .Select(x => x.Value)
                       .Sum();

            return ret;

        }

        /// <summary>
        /// Gives the sequence count for how many times This Fighter has successfully scored
        /// against Opponent Fighter (counterparry does not count as a score)
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        //protected int numberPreviousSuccessfulStrikes(string thisFighterId, string opponentFighterId)
        protected int numberPreviousSuccessfulStrikes(string thisFighterId)
        {
            int ret = 0;

            string opponentFighterId = otherFighterId(thisFighterId);

            if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }

            for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = _combatSession.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.SWING &&
                        combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action == CombatEnums.REST
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gives the sequence count for how many times This Fighter has successfully healed
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        //protected int numberPreviousSuccessfulHeals(string thisFighterId, string opponentFighterId)
        protected int numberPreviousSuccessfulHeals(string thisFighterId)
        {
            int ret = 0;

            string opponentFighterId = otherFighterId(thisFighterId);

            if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


            for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = _combatSession.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.REST &&
                        combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action != CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gives the sequence count for how many times This Fighter has successfully Blocked
        /// against Opponent Fighter (blocking a heal does not count)
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        //protected int numberPreviousSuccessfulBlocks(string thisFighterId, string opponentFighterId)
        protected int numberPreviousSuccessfulBlocks(string thisFighterId)
        {
            int ret = 0;
            string opponentFighterId = otherFighterId(thisFighterId);

            if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


            for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = _combatSession.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
                        combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action == CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gives the sequence count for how many times This Fighter has successfully Blocked
        /// against Opponent Fighter (blocking a heal does not count)
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        //protected int numberPreviousFalseBlocks(string thisFighterId, string opponentFighterId)
        protected int numberPreviousFalseBlocks(string thisFighterId)
        {
            int ret = 0;
            string opponentFighterId = otherFighterId(thisFighterId);

            if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }


            for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = _combatSession.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
                        combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action != CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

    }
}
