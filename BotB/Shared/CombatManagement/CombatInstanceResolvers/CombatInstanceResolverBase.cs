using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    public abstract class CombatInstanceResolverBase: ICombatInstanceResolver
    {
        protected CombatSession _combatSession;
        //protected string _thisFighterId = "";
        //protected string _opponentFighterId = "";

        public CombatInstanceResolverBase(CombatSession Session) 
        {
            _combatSession = Session;
        }

        public virtual CombatResult Resolve(CombatMove OpponentMove) 
        {

            string opponentFighterId = OpponentMove.FighterId;
            string thisFighterId = _combatSession.CombatRounds[0].Moves
                                    .Where(x => x.FighterId != opponentFighterId)
                                    .FirstOrDefault().FighterId;

            CombatResult combatResult = new CombatResult();

            switch (OpponentMove.Action) 
            {
                case CombatEnums.SWING:
                    return resolveForSwing(thisFighterId, opponentFighterId);

                case CombatEnums.BLOCK:
                    return resolveForBlock(thisFighterId, opponentFighterId);

                case CombatEnums.REST:
                    return resolveForHeal(thisFighterId, opponentFighterId);

            }

            return combatResult;
        }
    
        protected abstract CombatResult resolveForSwing(string ThisFighterId, string OpponentFighterId);
        protected abstract CombatResult resolveForBlock(string ThisFighterId, string OpponentFighterId);
        protected abstract CombatResult resolveForHeal(string ThisFighterId, string OpponentFighterId);

        protected int totalHPs(string fighterId) 
        {

            var ret = _combatSession.CombatRounds.Where(x => x.Result != null)
                       .SelectMany(x=>x.Result.HPAdjustment)
                       .Where(x => x.Key == fighterId)
                       .Select(x => x.Value)
                       .Sum();

            return ret;

        }
    }
}
