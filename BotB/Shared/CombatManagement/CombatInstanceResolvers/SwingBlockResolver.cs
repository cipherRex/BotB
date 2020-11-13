using BotB.Shared.CombatManagement.CombatHistoryResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class SwingBlockResolver : CombatInstanceResolverBase
    {

        public SwingBlockResolver(CombatSession Session) : base(Session)
        { }

        //public override CombatResult Resolve(CombatMove OpponentMove)
        //{
        //    return base.Resolve(OpponentMove);
        //}

        /// <summary>
        /// This Fighter SWINGS, Opponent Fighter BLOCKS
        /// If This Fighter has been blocked previously then
        /// he cant swing again next turn
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(List<CombatMove> Moves)
        {

            string swingingFighterId = Moves.Where(x => x.Action == CombatActions.SWING).FirstOrDefault().FighterId;
            string blockingFighterId = Moves.Where(x => x.Action == CombatActions.BLOCK).FirstOrDefault().FighterId;

            CombatResult combatResult = new CombatResult();
            ICombatHistoryResolver successfulBlockHistoryResolver = new SuccessfulBlockHistoryResolver(_combatSession);

            string comments;

            //get count of how may times Opponent Fighter has previously been blocked (consecutively)
            //int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId, opponentFighterId);
            //int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId);
            //int numberPreviousTimesBlocked = successfulBlockHistoryResolver.Resolve(swingingFighterId);
            int numberPreviousTimesBlocked = successfulBlockHistoryResolver.Resolve(blockingFighterId);

            //combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_SWING;
            //combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_BLOCK;
            combatResult.CombatAnimationInstructions.Add(swingingFighterId, new CombatAnimationInstruction() {FighterID = swingingFighterId , AnimCommand = AnimationCommands.AC_SWING });
            combatResult.CombatAnimationInstructions.Add(blockingFighterId, new CombatAnimationInstruction() { FighterID = blockingFighterId, AnimCommand = AnimationCommands.AC_BLOCK });

            //add flag to signal recoil animation
            combatResult.ShieldRecoil.Add(blockingFighterId);

            comments = string.Format("{0} blocks.", blockingFighterId);

            //If This Player was blocked previously then trigger taunting animation and restrict next move:
            if (numberPreviousTimesBlocked >= 1)
            {
                combatResult.ShieldTaunt.Add(blockingFighterId);
                //This Player cant swing next turn
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(swingingFighterId, CombatActions.SWING));
                //So opponent cant block
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(blockingFighterId, CombatActions.BLOCK));

                comments = comments + string.Format(" {0} cannot swing next turn, so {1} cannot use shield", swingingFighterId, blockingFighterId);
            }

            //combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
            //combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId);
            combatResult.TotalRunningHPs.Add(swingingFighterId, totalHPs(swingingFighterId));
            combatResult.TotalRunningHPs.Add(blockingFighterId, totalHPs(blockingFighterId));

            combatResult.Comments = comments;

            return combatResult;
        }


    }
}
