using BotB.Shared;
using System.Collections.Generic;

namespace BotB.Server.Hubs
{
    public class Arena
    {
        List<Fighter> _fighters = new List<Fighter>();

        public List<Fighter> Fighters()
        {
            return _fighters;
        }

        public void AddFighter(Fighter fighter)
        {
            _fighters.Add(fighter);
        }

        public void RemoveFighter(Fighter fighter)
        {
            _fighters.Remove(fighter);
        }
    }
}
