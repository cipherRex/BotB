using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    class SuccessfulBlockHistoryResolver: CombatHistoryResolverBase
    {
        public SuccessfulBlockHistoryResolver(CombatSession Session) : base(Session)
        { }

        protected override bool truthCondition(CombatActions thisPlayerAction, CombatActions opponentPlayerAction)
        {
            return thisPlayerAction == CombatActions.BLOCK && opponentPlayerAction == CombatActions.SWING;
        }
    }
}
