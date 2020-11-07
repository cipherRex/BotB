using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public class SwingInstanceResolver : CombatInstanceResolverBase
    {

        public SwingInstanceResolver(CombatSession Session) : base(Session)
        { 
        
        }

        public override CombatResult Resolve(CombatMove OpponentMove)
        {
            return base.Resolve(OpponentMove);
        }

        protected override CombatResult resolveForBlock(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId, opponentFighterId);

            combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_SWING;
            combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_BLOCK;

            if (numberPreviousTimesBlocked > 1)
            {
                
                
            }


      

            return combatResult;
        }

        protected override CombatResult resolveForHeal(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId, opponentFighterId);

            if (previousSuccessfulStrikes == 0)
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_KICK;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_GROINED;
            }
            else
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_CLEAVE;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_CLEAVED;
            }

            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);

            int totalOpponentDamage = -2 - previousSuccessfulStrikes;
            combatResult.HPAdjustment[opponentFighterId] = totalOpponentDamage;
            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) - totalOpponentDamage;

            return combatResult;
        }

        protected override CombatResult resolveForSwing(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            bool randomBool = new Random().Next(0, 2) > 0;
            string randomWinnerFighterId = randomBool ? thisFighterId : opponentFighterId;
            string randomLoserFighterId = randomBool ? opponentFighterId : thisFighterId;

            combatResult.CombatAnimationInstructions[randomWinnerFighterId].AnimCommand = AnimationCommand.AC_COUNTERPARRY;
            combatResult.CombatAnimationInstructions[randomLoserFighterId].AnimCommand = AnimationCommand.AC_PARRY;
            combatResult.HPAdjustment[randomLoserFighterId] = -1;
            combatResult.TotalRunningHPs[randomWinnerFighterId] = totalHPs(randomWinnerFighterId);
            combatResult.TotalRunningHPs[randomLoserFighterId] = totalHPs(randomLoserFighterId) - 1;

            combatResult.Comments = "Both knights swing. Random damage.";

            return combatResult;
        }


    }
}
