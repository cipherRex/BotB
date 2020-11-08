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

            string opponentFighterId = OpponentMove.FighterId;
            string thisFighterId = _combatSession.CombatRounds[0].Moves
                                    .Where(x => x.FighterId != opponentFighterId)
                                    .FirstOrDefault().FighterId;

            return resolve(thisFighterId, opponentFighterId);

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
        protected int numberPreviousSuccessfulStrikes(string thisFighterId, string opponentFighterId)
        {
            int ret = 0;

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
        protected int numberPreviousSuccessfulHeals(string thisFighterId, string opponentFighterId)
        {
            int ret = 0;

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
        protected int numberPreviousSuccessfulBlocks(string thisFighterId, string opponentFighterId)
        {
            int ret = 0;

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
        protected int numberPreviousFalseBlocks(string thisFighterId, string opponentFighterId)
        {
            int ret = 0;

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
