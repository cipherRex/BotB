using BotB.Shared.CombatManagement.CombatHistoryResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class RestRestResolver : CombatInstanceResolverBase
    {

        public RestRestResolver(CombatSession Session) : base(Session)
        { }

        public override CombatResult Resolve(CombatMove OpponentMove)
        {
            return base.Resolve(OpponentMove);
        }

        /// <summary>
        /// This Fighter RESTS, Opponent Fighter RESTS
        /// Check previous heals on both fighters 
        /// Both heal points with bonus if applicable
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            ICombatHistoryResolver successfulHealHistoryResolver = new SuccessfulHealHistoryResolver(_combatSession);

            //get count of how may times This Fighter has been successfully healed consecutively
            //int totalThisHealPoints = 1 + numberPreviousSuccessfulHeals(thisFighterId, opponentFighterId);
            //int totalThisHealPoints = 1 + numberPreviousSuccessfulHeals(thisFighterId);
            int totalThisHealPoints = 1 + successfulHealHistoryResolver.Resolve(thisFighterId);

            //get count of how may times Opponent Fighter has been successfully healed consecutively
            //int totalOpponentHealPoints = numberPreviousSuccessfulHeals(opponentFighterId, thisFighterId);
            //int totalOpponentHealPoints = numberPreviousSuccessfulHeals(opponentFighterId);
            int totalOpponentHealPoints = successfulHealHistoryResolver.Resolve(opponentFighterId);

            combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_HEAL;
            combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_HEAL;

            combatResult.HPAdjustment[thisFighterId] = totalThisHealPoints;
            combatResult.HPAdjustment[opponentFighterId] = 1 + totalOpponentHealPoints;

            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId) + totalThisHealPoints;
            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) + totalOpponentHealPoints;

            combatResult.Comments = "Both knights heal.";

            return combatResult;
        }
    }
}
