using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.Repositories.Fighter
{
    public interface IFighterRepository
    {
        List<BotB.Shared.Fighter> GetAll(string playerEmail);
    }
}
