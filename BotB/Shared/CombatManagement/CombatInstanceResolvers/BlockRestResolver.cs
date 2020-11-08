using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class BlockRestResolver : CombatInstanceResolverBase
    {

        public BlockRestResolver(CombatSession Session) : base(Session)
        { }

        public override CombatResult Resolve(CombatMove OpponentMove)
        {
            return base.Resolve(OpponentMove);
        }

        /// <summary>
        /// This Fighter BLOCKS, Opponent Fighter RESTS
        /// If This Fighter false blocked previously then 
        /// cant block next turn.
        /// Opponent Fighter heals
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();
            string comments = "Player heals.";

            //get count of how may times This Fighter has been previously false blocked
            //int previousFalseBlocks = numberPreviousFalseBlocks(thisFighterId, opponentFighterId);
            int previousFalseBlocks = numberPreviousFalseBlocks(thisFighterId);
            if (previousFalseBlocks > 0) 
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatEnums>( thisFighterId, CombatEnums.BLOCK));
                comments = string.Format("{0} false blocks and cannot block next turn", thisFighterId);
            }

            //get count of how may times Opponent Fighter has previously healed
            //int previousHeals = numberPreviousSuccessfulHeals(opponentFighterId, thisFighterId);
            int previousHeals = numberPreviousSuccessfulHeals(opponentFighterId);

            combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_BLOCK;
            combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_HEAL;

            //damage is base 2 plus previous consecutive hits
            int totalHealing = 1 + previousHeals;
            combatResult.HPAdjustment[opponentFighterId] = totalHealing;

            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) + totalHealing;
            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);

            combatResult.Comments = comments;

            return combatResult;
        }


    }
}
