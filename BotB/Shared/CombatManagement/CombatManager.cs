using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        }

        public CombatSession CreateCombatSession(Fighter Fighte1, Fighter Fighter2)
        {
            CombatSession combatSession = new CombatSession(Fighte1, Fighter2);
            CombatSessions.Add(combatSession);
            return combatSession;
        }

        private readonly Mutex _mutex = new Mutex();
        public bool setSemaphore(string fighterId)
        {
            _mutex.WaitOne();
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
                _mutex.ReleaseMutex();
            }
        }
    }
}
