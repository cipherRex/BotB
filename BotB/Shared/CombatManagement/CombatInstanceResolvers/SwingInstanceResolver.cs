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

        protected override CombatResult resolveForBlock(string ThisFighterId, string OpponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            bool randomBool = new Random().Next(0, 2) > 0;
            string randomWinnerFighterId = randomBool ? ThisFighterId : OpponentFighterId;
            string randomLoserFighterId = randomBool ? OpponentFighterId : ThisFighterId;

            combatResult.CombatAnimationInstructions[randomWinnerFighterId].AnimCommand = AnimationCommand.AC_COUNTERPARRY;
            combatResult.CombatAnimationInstructions[randomLoserFighterId].AnimCommand = AnimationCommand.AC_PARRY;
            combatResult.HPAdjustment[randomLoserFighterId] = -1;

            combatResult.Comments = "Both knights swing. Random damage.";
            
            return combatResult;
        }

        protected override CombatResult resolveForHeal(string thisFighterId, string opponentFighterId)
        {
            CombatResult combatResult = new CombatResult();

            int numberPreviousSuccessfulStrikes = NumberPreviousSuccessfulStrikes(thisFighterId, opponentFighterId);

            if (numberPreviousSuccessfulStrikes == 0)
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_KICK;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_GROINED;
            }
            else
            {
                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_CLEAVE;
                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_CLEAVED;
            }

            combatResult.HPAdjustment[opponentFighterId] = -2 - numberPreviousSuccessfulStrikes;

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

            combatResult.Comments = "Both knights swing. Random damage.";

            return combatResult;
        }

        protected int NumberPreviousSuccessfulStrikes(string thisFighterId, string opponentFighterId)
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

    }
}
