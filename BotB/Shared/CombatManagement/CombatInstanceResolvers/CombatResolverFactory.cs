using System;
using System.Collections.Generic;
using System.Text;

namespace BotB.Shared.CombatManagement.CombatInstanceResolvers
{
    class CombatResolverFactory
    {

        CombatSession _combatSession = null;

        public CombatResolverFactory(CombatSession combatSession) 
        {
            _combatSession = combatSession;
        }

        public ICombatInstanceResolver GetCombatResolver(List<CombatMove> Moves) 
        {

            const int FIRST_PLAYER = 0;
            const int SECOND_PLAYER = 1;

            if (Moves[FIRST_PLAYER].Action == CombatEnums.SWING && 
                Moves[SECOND_PLAYER].Action == CombatEnums.SWING) 
            {
                return new SwingSwingResolver(_combatSession);
            }
            else if (Moves[FIRST_PLAYER].Action == CombatEnums.SWING &&
                Moves[SECOND_PLAYER].Action == CombatEnums.BLOCK) 
            {
                return new SwingBlockResolver(_combatSession);
            }
            else if (Moves[FIRST_PLAYER].Action == CombatEnums.SWING &&
                Moves[SECOND_PLAYER].Action == CombatEnums.REST)
            {
                return new SwingRestResolver(_combatSession);
            }
            else if (Moves[FIRST_PLAYER].Action == CombatEnums.BLOCK &&
                Moves[SECOND_PLAYER].Action == CombatEnums.BLOCK)
            {
                return new BlockBlockResolver(_combatSession);
            }
            else if (Moves[FIRST_PLAYER].Action == CombatEnums.BLOCK &&
                Moves[SECOND_PLAYER].Action == CombatEnums.REST)
            {
                return new BlockRestResolver(_combatSession);
            }
            else if (Moves[FIRST_PLAYER].Action == CombatEnums.REST &&
                Moves[SECOND_PLAYER].Action == CombatEnums.REST)
            {
                return new RestRestResolver(_combatSession);
            }
            else 
            {
                throw new Exception("Unknown combat pattern: ");
            }

        }
    }
}
