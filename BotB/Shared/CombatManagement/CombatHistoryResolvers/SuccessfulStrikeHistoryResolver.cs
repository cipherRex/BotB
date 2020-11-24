using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    public class SuccessfulStrikeHistoryResolver : CombatHistoryResolverBase
    {
        public SuccessfulStrikeHistoryResolver(CombatSession Session) : base(Session) 
        { }

        protected override bool truthCondition(CombatActions thisPlayerAction, CombatActions opponentPlayerAction)
        {
            return thisPlayerAction == CombatActions.SWING && opponentPlayerAction == CombatActions.REST;
        }
    }
}
