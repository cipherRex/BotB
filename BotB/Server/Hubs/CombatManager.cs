using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BotB.Shared;
using BotB.Shared.CombatManagement;

namespace BotB.Server.Hubs
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

        public void DeleteSession(string FighterId) 
        {
            CombatSessions.Remove
                (
                    CombatSessions.Where(x => x.Fighters.Keys.Contains(FighterId)).FirstOrDefault()
                );
        }

        private readonly Mutex _semaphoreMutex = new Mutex();
        public bool setSemaphore(string fighterId)
        {
            _semaphoreMutex.WaitOne();
            try
            {
                CombatSession thisCombatSession = GetCombatSessionByFighterId(fighterId);
                if (thisCombatSession.AnimationSemaphore.Count == 0)
                {
                    thisCombatSession.AnimationSemaphore.Add(fighterId, true);
                    return false;
                }
                else if (thisCombatSession.AnimationSemaphore.Count == 1)
                {
                    if (thisCombatSession.AnimationSemaphore.ContainsKey(fighterId))
                    {
                        return false;
                        // throw new Exception("AnimationSemaphore exception");
                    }
                    else
                    {
                        thisCombatSession.AnimationSemaphore = new Dictionary<string, bool>();
                        return true;
                    }
                }
                else
                {
                    throw new Exception("AnimationSemaphore exception");
                }
            }
            finally
            {
                _semaphoreMutex.ReleaseMutex();
            }
        }

    }
}
