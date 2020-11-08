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
        protected override CombatResult resolve(string thisFighterId, string opponentFighterId, ICombatHistoryResolver successfulBlockHistoryResolver)
        {
            CombatResult combatResult = new CombatResult();
            //ICombatHistoryResolver successfulBlockHistoryResolver = new SuccessfulBlockHistoryResolver(_combatSession);

            string comments = "";

            //get count of how may times Opponent Fighter has previously been blocked (consecutively)
            //int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId, opponentFighterId);
            //int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId);
            int numberPreviousTimesBlocked = successfulBlockHistoryResolver.Resolve(thisFighterId);

            combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_SWING;
            combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_BLOCK;

            //add flag to signal recoil animation
            combatResult.ShieldRecoil.Add(opponentFighterId);

            comments = string.Format("{0} blocks.", opponentFighterId);

            //If This Player was blocked previously then trigger taunting animation and restrict next move:
            if (numberPreviousTimesBlocked > 1)
            {
                combatResult.ShieldTaunt.Add(opponentFighterId);
                //This Player cant swing next turn
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(thisFighterId, CombatActions.SWING));
                //So opponent cant block
                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatActions>(opponentFighterId, CombatActions.BLOCK));

                comments = comments + string.Format(" {0} cannot swing next turn, so {1} cannot use shield", thisFighterId, opponentFighterId);
            }

            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId);

            combatResult.Comments = comments;

            return combatResult;
        }

        protected override CombatResult resolve(string ThisFighterId, string OpponentFighterId)
        {
            throw new NotImplementedException();
        }
    }
}
