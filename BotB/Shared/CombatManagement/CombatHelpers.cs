using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatHelpers
    {
        /// <summary>
        /// Pass in a figher id and session to find the other
        /// fighter's id
        /// </summary>
        /// <param name="thisFighterId"></param>
        /// <param name="combatSession"></param>
        /// <returns></returns>
        public static string otherFighterId(string thisFighterId, CombatSession combatSession)
        {
            return combatSession.CombatRounds[0].Moves
                    .Where(x => x.FighterId != thisFighterId)
                    .FirstOrDefault().FighterId;
        }
    }
}
