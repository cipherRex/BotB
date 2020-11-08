using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    public interface ICombatHistoryResolver 
    {
        int Resolve(string ThisFighterId);
    }

    public abstract class CombatHistoryResolverBase: ICombatHistoryResolver
    {
        protected CombatSession _combatSession;

        protected abstract bool truthCondition(CombatActions thisPlayerAction, CombatActions opponentPlayerAction);

        public CombatHistoryResolverBase(CombatSession Session)
        {
            _combatSession = Session;
        }

        public virtual int Resolve(string ThisFighterId)
        {

            int ret = 0;

            //string opponentFighterId = otherFighterId(ThisFighterId);
            string opponentFighterId = CombatHelpers.otherFighterId(ThisFighterId, _combatSession);

            if (_combatSession.CombatRounds.Where(x => x.Result != null).Count() == 0) { return 0; }

            for (int i = _combatSession.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = _combatSession.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    //if (combatRound.Moves.Where(x => x.FighterId == ThisFighterId).FirstOrDefault().Action == CombatEnums.SWING &&
                    //    combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action == CombatEnums.REST
                    //)
                    if (truthCondition
                        (
                            combatRound.Moves.Where(x => x.FighterId == ThisFighterId).FirstOrDefault().Action,
                            combatRound.Moves.Where(x => x.FighterId == opponentFighterId).FirstOrDefault().Action
                        )
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

        //private string otherFighterId(string thisFighterId)
        //{
        //    return _combatSession.CombatRounds[0].Moves
        //            .Where(x => x.FighterId != thisFighterId)
        //            .FirstOrDefault().FighterId;
        //}
    }
}
