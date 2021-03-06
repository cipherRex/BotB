﻿using BotB.Shared.CombatManagement.CombatHistoryResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class BlockBlockResolver : CombatInstanceResolverBase
    {

        public BlockBlockResolver(CombatSession Session) : base(Session)
        { }

        //public override CombatResult Resolve(CombatMove OpponentMove)
        //{
        //    return base.Resolve(OpponentMove);
        //}

        /// <summary>
        /// This Fighter BLOCKS, Opponent Fighter BLOCKS
        /// If either false blocked previously then 
        /// cant block next turn
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(List<CombatMove> Moves)
        {
            string thisFighterId = Moves[0].FighterId;
            string opponentFighterId = Moves[1].FighterId;

            CombatResult combatResult = new CombatResult();
            string comments = "Both knights block.";

            //ICombatHistoryResolver successfulBlockHistoryResolver = new SuccessfulBlockHistoryResolver(_combatSession);
            ICombatHistoryResolver falseBlockHistoryResolver = new FalseBlockHistoryResolver(_combatSession);

            //int numTimesThisFighterHasBeenBlocked = numberPreviousSuccessfulBlocks(opponentFighterId, thisFighterId);
            //int numTimesOpponentFighterHasBeenBlocked = numberPreviousSuccessfulBlocks(thisFighterId, opponentFighterId);
            //int numTimesThisFighterHasBeenBlocked = numberPreviousSuccessfulBlocks(opponentFighterId);
            //int numTimesOpponentFighterHasBeenBlocked = numberPreviousSuccessfulBlocks(thisFighterId);
            int numTimesThisFighterHasFalseBlocked = falseBlockHistoryResolver.Resolve(thisFighterId);
            int numTimesOpponentFighterHasFalseBlocked = falseBlockHistoryResolver.Resolve(opponentFighterId);

            //combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_BLOCK;
            //combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_BLOCK;
            combatResult.CombatAnimationInstructions.Add(thisFighterId,
                new CombatAnimationInstruction() { FighterID = thisFighterId, AnimCommand = AnimationCommands.AC_BLOCK });
            combatResult.CombatAnimationInstructions.Add(opponentFighterId,
                new CombatAnimationInstruction() { FighterID = opponentFighterId, AnimCommand = AnimationCommands.AC_BLOCK });

            if (numTimesThisFighterHasFalseBlocked > 0)
            {
                comments = comments + string.Format(" {0} false blocked previoulsy, cannot block next turn. ", thisFighterId);
                //combatResult.ShieldTaunt.Add(opponentFighterId);
                //This Fighter cant block next turn
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.BLOCK));
                //So Opponent Fighter cant swing either
                //combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(opponentFighterId, CombatActions.SWING));
            }

            if (numTimesOpponentFighterHasFalseBlocked > 0)
            {
                comments = comments + string.Format(" {0} false blocked previoulsy, cannot block next turn. ", opponentFighterId);
                //combatResult.ShieldTaunt.Add(thisFighterId);
                //Opponent Fighter cant block next turn
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(opponentFighterId, CombatActions.BLOCK));
                //So This Fighter cant block either
                //combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.BLOCK));
            }

            //combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
            //combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId);
            combatResult.TotalRunningHPs.Add(thisFighterId, totalHPs(thisFighterId));
            combatResult.TotalRunningHPs.Add(opponentFighterId, totalHPs(opponentFighterId));

            combatResult.Comments = comments;

            return combatResult;
        }


    }
}
