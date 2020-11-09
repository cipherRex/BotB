using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotB.Shared.CombatManagement
{
    public class CombatManager
    {
        List<CombatSession> CombatSessions { get; set; }

        public CombatManager() 
        {
            CombatSessions = new List<CombatSession>();
        }

        public CombatSession GetCombatSessionByFighterId(string fighterId) 
        {

            return CombatSessions.Where(x => x.Fighters.ContainsKey(fighterId)).FirstOrDefault();

            //foreach(CombatSession combatSession in CombatSessions) 
            //{ 
            //    if (combatSession.CombatRounds[0].Moves[0].FighterId == fighterId ||
            //        combatSession.CombatRounds[0].Moves[1].FighterId == fighterId
            //        )
            //    {
            //        //return new CombatSession() {CombatRounds = combatSession.CombatRounds.ToList() };
            //        return combatSession;
            //    }
            //}

            //return null;
        }

        public CombatSession CreateCombatSession(string fighter1Id, string fighter2Id )
        {
            CombatSession combatSession = new CombatSession(fighter1Id, fighter2Id);
            CombatSessions.Add(combatSession);
            return combatSession;
        }
    }
}
