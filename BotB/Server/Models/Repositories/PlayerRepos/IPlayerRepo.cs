using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.Repositories.PlayerRepos
{
    public interface IPlayerRepo
    {
        Player GetPlayer(string Email);
    }
}
