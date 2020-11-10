using BotB.Shared.CombatManagement.CombatHistoryResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class SwingSwingResolver : CombatInstanceResolverBase
    {

        public SwingSwingResolver(CombatSession Session) : base(Session)
        { }

        //public override CombatResult Resolve(CombatMove OpponentMove)
        //{
        //    return base.Resolve(OpponentMove);
        //}

        /// <summary>
        /// This Fighter SWINGS, Opponent Fighter SWINGS.
        /// One of them takes 1 random point of damage.
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="opponentFighterId"></param>
        /// <returns></returns>
        protected override CombatResult resolve(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            //randomly determine which fighter is the loser:
            bool randomBool = new Random().Next(0, 2) > 0;
            string randomWinnerFighterId = randomBool ? thisFighterId : opponentFighterId;
            string randomLoserFighterId = randomBool ? opponentFighterId : thisFighterId;

            //winner counterparries (ie, wins)
            //combatResult.CombatAnimationInstructions[randomWinnerFighterId].AnimCommand = AnimationCommands.AC_COUNTERPARRY;
            //combatResult.CombatAnimationInstructions[randomLoserFighterId].AnimCommand = AnimationCommands.AC_PARRY;
            combatResult.CombatAnimationInstructions.Add(
                randomWinnerFighterId, 
                new CombatAnimationInstruction() 
                    {FighterID = randomWinnerFighterId , AnimCommand = AnimationCommands.AC_COUNTERPARRY}
                );
            combatResult.CombatAnimationInstructions.Add(
                randomLoserFighterId,
                new CombatAnimationInstruction()
                { FighterID = randomLoserFighterId, AnimCommand = AnimationCommands.AC_PARRY }
                );

            //loser loses a point
            //combatResult.HPAdjustments[randomLoserFighterId] = -1;
            combatResult.HPAdjustments.Add(randomLoserFighterId, -1);

            //combatResult.TotalRunningHPs[randomWinnerFighterId] = totalHPs(randomWinnerFighterId);
            //combatResult.TotalRunningHPs[randomLoserFighterId] = totalHPs(randomLoserFighterId) - 1;
            combatResult.TotalRunningHPs.Add(randomWinnerFighterId, totalHPs(randomWinnerFighterId));
            combatResult.TotalRunningHPs.Add(randomLoserFighterId, totalHPs(randomLoserFighterId) - 1);

            combatResult.Comments = string.Format("Both knights swing. {0} counterparries and {1} takes damage.", thisFighterId, randomLoserFighterId) ;

            return combatResult;
        }


    }
}
