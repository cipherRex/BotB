using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotB.Shared.CombatManagement;

namespace BotB.Shared.Hubs
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

        public CombatSession CreateCombatSession(Fighter Fighte1, Fighter Fighter2)
        {
            CombatSession combatSession = new CombatSession(Fighte1, Fighter2);
            CombatSessions.Add(combatSession);
            return combatSession;
        }
    }
}
