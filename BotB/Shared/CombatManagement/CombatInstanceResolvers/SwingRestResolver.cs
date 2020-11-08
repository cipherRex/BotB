using BotB.Shared.CombatManagement.CombatHistoryResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class SwingRestResolver : CombatInstanceResolverBase
    {

        public SwingRestResolver(CombatSession Session) : base(Session)
        { }

        //public override CombatResult Resolve(CombatMove OpponentMove)
        //{
        //    return base.Resolve(OpponentMove);
        //}

        /// <summary>
        /// This Fighter SWINGS, Opponent Fighter (attempts to) REST
        /// If Opponent Fighter has been dit during reat previously, 
        /// takes additional damage
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();
            ICombatHistoryResolver successfulStrikeHistoryResolver = new SuccessfulStrikeHistoryResolver(_combatSession);

            //get count of how may times Opponent Fighter has previously been hit (consecutively)
            //int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId, opponentFighterId);
            //int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId);
            int previousSuccessfulStrikes = successfulStrikeHistoryResolver.Resolve(thisFighterId);

            //if no previous hits then MINOR damage animations
            if (previousSuccessfulStrikes == 0)
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_KICK;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_GROINED;

                combatResult.Comments = string.Format("{0} takes damage.", opponentFighterId);

            }
            //if there are previous consecutive hits, them MAJOR damage animations
            else
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_CLEAVE;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_CLEAVED;

                combatResult.Comments = string.Format("{0} takes lots of damage.", opponentFighterId);

            }

            //damage is base 2 plus previous consecutive hits
            int totalOpponentDamage = -2 - previousSuccessfulStrikes;
            combatResult.HPAdjustment[opponentFighterId] = totalOpponentDamage;

            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) - totalOpponentDamage;
            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);


            return combatResult;
        }


    }
}
