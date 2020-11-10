using BotB.Shared.CombatManagement.CombatHistoryResolvers;
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

        //public override CombatResult Resolve(CombatMove OpponentMove)
        //{
        //    return base.Resolve(OpponentMove);
        //}

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

            ICombatHistoryResolver falseBlockHistoryResolver = new FalseBlockHistoryResolver(_combatSession);

            //get count of how may times This Fighter has been previously false blocked
            //int previousFalseBlocks = numberPreviousFalseBlocks(thisFighterId, opponentFighterId);
            //int previousFalseBlocks = numberPreviousFalseBlocks(thisFighterId);
            int previousFalseBlocks = falseBlockHistoryResolver.Resolve(thisFighterId);
            if (previousFalseBlocks > 0) 
            {
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>( thisFighterId, CombatActions.BLOCK));
                comments = string.Format("{0} false blocks and cannot block next turn", thisFighterId);
            }

            //get count of how may times Opponent Fighter has previously healed
            //int previousHeals = numberPreviousSuccessfulHeals(opponentFighterId, thisFighterId);
            //int previousHeals = numberPreviousSuccessfulHeals(opponentFighterId);
            int previousHeals = falseBlockHistoryResolver.Resolve(opponentFighterId);

            //combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_BLOCK;
            //combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_HEAL;
            combatResult.CombatAnimationInstructions.Add(thisFighterId, new CombatAnimationInstruction() 
            {FighterID = thisFighterId , AnimCommand = AnimationCommands.AC_BLOCK});
            combatResult.CombatAnimationInstructions.Add(opponentFighterId, new CombatAnimationInstruction()
            { FighterID = opponentFighterId, AnimCommand = AnimationCommands.AC_HEAL });


            //damage is base 2 plus previous consecutive hits
            int totalHealing = 1 + previousHeals;
            //combatResult.HPAdjustments[opponentFighterId] = totalHealing;
            combatResult.HPAdjustments.Add(opponentFighterId, totalHealing);

            //combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) + totalHealing;
            //combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
            combatResult.TotalRunningHPs.Add(opponentFighterId,  totalHPs(opponentFighterId) + totalHealing);
            combatResult.TotalRunningHPs.Add(thisFighterId, totalHPs(thisFighterId));

            combatResult.Comments = comments;

            return combatResult;
        }


    }
}
