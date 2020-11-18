using BotB.Server.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.Repositories.Fighter
{
    public class FighterRepository : IFighterRepository
    {
        SqlDAL _sqlDAL;
        public FighterRepository(SqlDAL SqlDAL) 
        {
            _sqlDAL = SqlDAL;
        }

        public List<Shared.Fighter> GetAll(string playerEmail)
        {
            return (
                from DataRow dr in _sqlDAL.GetPlayerFighters(playerEmail).Rows
                select new Shared.Fighter() 
                { 
                    id = dr["FighterID"].ToString(),
                    Name = dr["FighterName"].ToString(),
                    Picture = dr["PictureFilename"].ToString()
                }
                ).ToList<Shared.Fighter>();
        }
    }
}
