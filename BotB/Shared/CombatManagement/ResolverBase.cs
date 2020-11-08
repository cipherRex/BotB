//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace BotB.Shared.CombatManagement
//{
//    public abstract class ResolverBase<T, U>
//    {
//        CombatSession _combatSession = null;

//        public ResolverBase(CombatSession Session) 
//        {
//            _combatSession = Session;
//        }

//        protected abstract T Resolve(U Param) ;

//        protected string otherFighterId(string thisFighterId)
//        {
//            return _combatSession.CombatRounds[0].Moves
//                    .Where(x => x.FighterId != thisFighterId)
//                    .FirstOrDefault().FighterId;
//        }
//    }
//}
