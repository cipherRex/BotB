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
        protected override CombatResult resolve(List<CombatMove> Moves)
        {
            const int BASE_DAMAGE = 2;

            string swingingFighterId = Moves.Where(x=> x.Action == CombatActions.SWING).FirstOrDefault().FighterId;
            string restingFighterId = Moves.Where(x => x.Action == CombatActions.REST).FirstOrDefault().FighterId;

            CombatResult combatResult = new CombatResult();
            ICombatHistoryResolver successfulStrikeHistoryResolver = new SuccessfulStrikeHistoryResolver(_combatSession);

            //get count of how may times Opponent Fighter has previously been hit (consecutively)
            //int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId, opponentFighterId);
            //int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId);
            int previousSuccessfulStrikes = successfulStrikeHistoryResolver.Resolve(swingingFighterId);

            //if no previous hits then MINOR damage animations
            if (previousSuccessfulStrikes == 0)
            {
                //combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_KICK;
                //combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_GROINED;
                combatResult.CombatAnimationInstructions.Add(
                    swingingFighterId,
                    new CombatAnimationInstruction()
                    { FighterID = swingingFighterId, AnimCommand = AnimationCommands.AC_KICK }
                    );
                combatResult.CombatAnimationInstructions.Add(
                    restingFighterId,
                    new CombatAnimationInstruction()
                    { FighterID = restingFighterId, AnimCommand = AnimationCommands.AC_GROINED }
                    );

                combatResult.Comments = string.Format("{0} takes damage.", restingFighterId);

            }
            //if there are previous consecutive hits, them MAJOR damage animations
            else
            {
                //combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommands.AC_CLEAVE;
                //combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommands.AC_CLEAVED;
                combatResult.CombatAnimationInstructions.Add(
                    swingingFighterId,
                    new CombatAnimationInstruction()
                    { FighterID = swingingFighterId, AnimCommand = AnimationCommands.AC_CLEAVE }
                    );
                combatResult.CombatAnimationInstructions.Add(
                    restingFighterId,
                    new CombatAnimationInstruction()
                    { FighterID = restingFighterId, AnimCommand = AnimationCommands.AC_CLEAVED }
                    );

                combatResult.Comments = string.Format("{0} takes lots of damage.", restingFighterId);

            }

            //damage is base 2 plus previous consecutive hits
            int totalOpponentDamage = BASE_DAMAGE + previousSuccessfulStrikes;
            //combatResult.HPAdjustments[opponentFighterId] = totalOpponentDamage;
            combatResult.HPAdjustments.Add(restingFighterId, totalOpponentDamage * -1);

            //combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) - totalOpponentDamage;
            //combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
            combatResult.TotalRunningHPs.Add(restingFighterId, totalHPs(restingFighterId) - totalOpponentDamage);
            combatResult.TotalRunningHPs.Add(swingingFighterId, totalHPs(swingingFighterId));

            return combatResult;
        }


    }
}
