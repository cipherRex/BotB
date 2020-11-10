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

            const int SWING_SWING = 1;
            const int SWING_BLOCK = 2;
            const int SWING_REST = 3;
            const int BLOCK_BLOCK = 4;
            const int BLOCK_REST = 6;
            const int REST_REST = 9;

            switch((int)Moves[FIRST_PLAYER].Action * 
                    (int)Moves[SECOND_PLAYER].Action) 
            {
                case SWING_SWING:
                    return new SwingSwingResolver(_combatSession);

                case SWING_BLOCK:
                    return new SwingBlockResolver(_combatSession);

                case SWING_REST:
                    return new SwingRestResolver(_combatSession);

                case BLOCK_BLOCK:
                    return new BlockBlockResolver(_combatSession);

                case BLOCK_REST:
                    return new BlockRestResolver(_combatSession);

                case REST_REST:
                    return new RestRestResolver(_combatSession);

                default:
                    throw new Exception("Unknown combat pattern: ");
            }


            //if (Moves[FIRST_PLAYER].Action == CombatActions.SWING &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.SWING)
            //{
            //    return new SwingSwingResolver(_combatSession);
            //}
            //else if (Moves[FIRST_PLAYER].Action == CombatActions.SWING &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.BLOCK)
            //{
            //    return new SwingBlockResolver(_combatSession);
            //}
            //else if (Moves[FIRST_PLAYER].Action == CombatActions.SWING &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.REST)
            //{
            //    return new SwingRestResolver(_combatSession);
            //}
            //else if (Moves[FIRST_PLAYER].Action == CombatActions.BLOCK &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.BLOCK)
            //{
            //    return new BlockBlockResolver(_combatSession);
            //}
            //else if (Moves[FIRST_PLAYER].Action == CombatActions.BLOCK &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.REST)
            //{
            //    return new BlockRestResolver(_combatSession);
            //}
            //else if (Moves[FIRST_PLAYER].Action == CombatActions.REST &&
            //    Moves[SECOND_PLAYER].Action == CombatActions.REST)
            //{
            //    return new RestRestResolver(_combatSession);
            //}
            //else
            //{
            //    throw new Exception("Unknown combat pattern: ");
            //}

        }
    }
}
