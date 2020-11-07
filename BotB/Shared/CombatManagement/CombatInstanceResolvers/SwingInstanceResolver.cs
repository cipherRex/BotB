//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
//{
//    public class SwingInstanceResolver : CombatInstanceResolverBase
//    {

//        public SwingInstanceResolver(CombatSession Session) : base(Session)
//        { }

//        public override CombatResult Resolve(CombatMove OpponentMove)
//        {
//            return base.Resolve(OpponentMove);
//        }

//        /// <summary>
//        /// This Fighter SWINGS, Opponent Fighter BLOCKS
//        /// If This Fighter has been blocked previously then
//        /// he cant swing again next turn
//        /// </summary>
//        /// <param name="thisFighterId"></param>
//        /// <param name="opponentFighterId"></param>
//        /// <returns></returns>
//        protected override CombatResult resolveForBlock(string thisFighterId, string opponentFighterId)
//        {
//            CombatResult combatResult = new CombatResult();

//            //get count of how may times Opponent Fighter has previously been blocked (consecutively)
//            int numberPreviousTimesBlocked = numberPreviousSuccessfulBlocks(thisFighterId, opponentFighterId);

//            combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_SWING;
//            combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_BLOCK;

//            //add flag to signal recoil animation
//            combatResult.ShieldRecoil.Add(opponentFighterId);

//            //If This Player was blocked previously then trigger taunting animation and restrict next move:
//            if (numberPreviousTimesBlocked > 1)
//            {
//                combatResult.ShieldTaunt.Add(opponentFighterId);
//                //This Player cant swing next turn
//                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatEnums>(thisFighterId, CombatEnums.SWING));
//                //So opponent cant block
//                combatResult.MoveRestrictions.Add(new KeyValuePair<string, CombatEnums>(opponentFighterId, CombatEnums.BLOCK));
//            }

//            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);
//            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId);

//            return combatResult;
//        }

//        /// <summary>
//        /// This Fighter SWINGS, Opponent Fighter (attempts to) REST
//        /// If Opponent Fighter has been dit during reat previously, 
//        /// takes additional damage
//        /// </summary>
//        /// <param name="thisFighterId"></param>
//        /// <param name="opponentFighterId"></param>
//        /// <returns></returns>
//        protected override CombatResult resolveForHeal(string thisFighterId, string opponentFighterId)
//        {
//            CombatResult combatResult = new CombatResult();

//            //get count of how may times Opponent Fighter has previously been hit (consecutively)
//            int previousSuccessfulStrikes = numberPreviousSuccessfulStrikes(thisFighterId, opponentFighterId);

//            //if no previous hits, them MINOR damage animations
//            if (previousSuccessfulStrikes == 0)
//            {
//                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_KICK;
//                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_GROINED;
//            }
//            //if there are previous consecutive hits, them MAJOR damage animations
//            else
//            {
//                combatResult.CombatAnimationInstructions[thisFighterId].AnimCommand = AnimationCommand.AC_CLEAVE;
//                combatResult.CombatAnimationInstructions[opponentFighterId].AnimCommand = AnimationCommand.AC_CLEAVED;
//            }

//            //damage is base 2 plus previous consecutive hits
//            int totalOpponentDamage = -2 - previousSuccessfulStrikes;
//            combatResult.HPAdjustment[opponentFighterId] = totalOpponentDamage;

//            combatResult.TotalRunningHPs[opponentFighterId] = totalHPs(opponentFighterId) - totalOpponentDamage;
//            combatResult.TotalRunningHPs[thisFighterId] = totalHPs(thisFighterId);

//            return combatResult;
//        }

//        /// <summary>
//        /// This Fighter SWINGS, Opponent Fighter SWINGS.
//        /// One of them takes 1 random point of damage.
//        /// </summary>
//        /// <param name="thisFighterId"></param>
//        /// <param name="opponentFighterId"></param>
//        /// <returns></returns>
//        protected override CombatResult resolveForSwing(string thisFighterId, string opponentFighterId)
//        {
//            CombatResult combatResult = new CombatResult();

//            //randomly determine which fighter is the loser:
//            bool randomBool = new Random().Next(0, 2) > 0;
//            string randomWinnerFighterId = randomBool ? thisFighterId : opponentFighterId;
//            string randomLoserFighterId = randomBool ? opponentFighterId : thisFighterId;

//            //winner counterparries (ie, wins)
//            combatResult.CombatAnimationInstructions[randomWinnerFighterId].AnimCommand = AnimationCommand.AC_COUNTERPARRY;
//            combatResult.CombatAnimationInstructions[randomLoserFighterId].AnimCommand = AnimationCommand.AC_PARRY;
            
//            //loser loses a point
//            combatResult.HPAdjustment[randomLoserFighterId] = -1;

//            combatResult.TotalRunningHPs[randomWinnerFighterId] = totalHPs(randomWinnerFighterId);
//            combatResult.TotalRunningHPs[randomLoserFighterId] = totalHPs(randomLoserFighterId) - 1;

//            combatResult.Comments = "Both knights swing. Random damage.";

//            return combatResult;
//        }
//    }
//}
