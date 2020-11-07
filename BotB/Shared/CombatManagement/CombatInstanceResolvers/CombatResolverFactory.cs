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

            CombatInstanceResolverBase combatInstanceResolverBase = null;

            switch (Moves[0].Action) 
            {
                case CombatEnums.SWING:
                    switch (Moves[1].Action) 
                    {
                        case CombatEnums.SWING:
                            combatInstanceResolverBase = new SwingSwingResolver(_combatSession);
                            break;

                        case CombatEnums.BLOCK:
                            combatInstanceResolverBase = new SwingBlockResolver(_combatSession);
                            break;

                        case CombatEnums.REST:
                            combatInstanceResolverBase = new SwingRestResolver(_combatSession);
                            break;
                    }
                    break;

                case CombatEnums.BLOCK:
                    switch (Moves[1].Action)
                    {
                        case CombatEnums.SWING:
                            combatInstanceResolverBase = new SwingBlockResolver(_combatSession);
                            break;

                        case CombatEnums.BLOCK:
                            combatInstanceResolverBase = new BlockBlockResolver(_combatSession);
                            break;

                        case CombatEnums.REST:
                            combatInstanceResolverBase = new BlockRestResolver(_combatSession);
                            break;
                    }
                    break;

                case CombatEnums.REST:
                    switch (Moves[1].Action)
                    {
                        case CombatEnums.SWING:
                            combatInstanceResolverBase = new SwingRestResolver(_combatSession);
                            break;

                        case CombatEnums.BLOCK:
                            combatInstanceResolverBase = new BlockRestResolver(_combatSession);
                            break;

                        case CombatEnums.REST:
                            combatInstanceResolverBase = new RestRestResolver(_combatSession);
                            break;
                    }
                    break;
            }

            return combatInstanceResolverBase;

        }
    }
}
