using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatHistoryResolvers
{
    class FalseBlockHistoryResolver: CombatHistoryResolverBase
    {
        public FalseBlockHistoryResolver(CombatSession Session) : base(Session)
        { }

        protected override bool truthCondition(CombatEnums thisPlayerAction, CombatEnums opponentPlayerAction)
        {
            return thisPlayerAction == CombatEnums.BLOCK && opponentPlayerAction != CombatEnums.SWING;
        }
    }
}
