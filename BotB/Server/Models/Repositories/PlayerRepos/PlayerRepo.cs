using BotB.Server.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.Repositories.PlayerRepos
{
    public class PlayerRepo : IPlayerRepo
    {
        IDAL _dal;
        public PlayerRepo(IDAL dal) 
        {
            _dal = dal;
        }

        public Player GetPlayer(string Email)
        {

            return
            (
                new Player()
                {
                    Email = Email,
                    Balance = Convert.ToInt32(_dal.GetPlayerInfo(Email).Rows[0]["Balance"])
                }
            ); ;
        }
    }
}
