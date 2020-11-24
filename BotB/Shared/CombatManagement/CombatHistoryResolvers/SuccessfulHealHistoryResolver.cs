using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    class SuccessfulHealHistoryResolver : CombatHistoryResolverBase
    {
        public SuccessfulHealHistoryResolver(CombatSession Session) : base(Session)
        { }

        protected override bool truthCondition(CombatActions thisPlayerAction, CombatActions opponentPlayerAction)
        {
            return thisPlayerAction == CombatActions.REST && opponentPlayerAction != CombatActions.SWING;
        }
    }
}
