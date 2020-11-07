//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
//{
//    public class BlockInstanceResolver : CombatInstanceResolverBase
//    {

//        public BlockInstanceResolver(CombatSession Session) : base(Session)
//        { }

//        public override CombatResult Resolve(CombatMove OpponentMove)
//        {
//            return base.Resolve(OpponentMove);
//        }

//        /// <summary>
//        /// This Fighter BLOCKS, Opponent Fighter SWINGS
//        /// If This Fighter has been blocked previously then
//        /// he cant swing again next turn
//        /// </summary>
//        /// <param name="thisFighterId"></param>
//        /// <param name="opponentFighterId"></param>
//        /// <returns></returns>
//        protected override CombatResult resolveForBlock(string thisFighterId, string opponentFighterId)
//        {
//            CombatResult combatResult = new CombatResult();

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



//            return combatResult;
//        }
//    }
//}
