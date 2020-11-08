using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    public class SuccessfulStrikeHistoryResolver : CombatHistoryResolverBase
    {
        public SuccessfulStrikeHistoryResolver(CombatSession Session) : base(Session) 
        { }

        protected override bool truthCondition(CombatEnums thisPlayerAction, CombatEnums opponentPlayerAction)
        {
            return thisPlayerAction == CombatEnums.SWING && opponentPlayerAction == CombatEnums.REST;
        }
    }
}
