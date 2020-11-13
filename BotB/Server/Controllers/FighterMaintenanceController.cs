
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BotB.Server.Models.UoW;
using BotB.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotB.Server.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class FighterMaintenanceController : ControllerBase
    {

        [HttpGet("PlayerFighters")]
        //public async Task<IEnumerable<Fighter>> PlayerFighters()
        public List<Fighter> PlayerFighters()
        {
            var _userEmail = userEmail().Result;

            using (IDbConnection dbConnection =
                  DbProviderFactories.GetFactory("system.data.sqlclient").CreateConnection())
            {
                dbConnection.ConnectionString = "Server=tcp:cipherrex.database.windows.net,1433;Initial Catalog=cipherRexUmbraco;Persist Security Info=False;User ID=cipherrex;Password=R00ksp@wnR00ksp@wn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                dbConnection.Open();
                Models.DAL.SqlDAL sqlDAL = new Models.DAL.SqlDAL(dbConnection);
                Models.Repositories.Fighter.IFighterRepository fighterRepository = new Models.Repositories.Fighter.FighterRepository(sqlDAL);
                return fighterRepository.GetAll(_userEmail);
            }

        }

        [HttpPost("NewFighter")]
        //public async Task<IEnumerable<Fighter>> PlayerFighters()
        public async void NewFighter([FromBody] string fighterJson)
        {
            var _userEmail = userEmail().Result;

            Fighter fighter = System.Text.Json.JsonSerializer.Deserialize<Fighter>(fighterJson);

            using (IDbConnection dbConnection =
                  DbProviderFactories.GetFactory("system.data.sqlclient").CreateConnection())
            {
                dbConnection.ConnectionString = "Server=tcp:cipherrex.database.windows.net,1433;Initial Catalog=cipherRexUmbraco;Persist Security Info=False;User ID=cipherrex;Password=R00ksp@wnR00ksp@wn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                dbConnection.Open();
                Models.DAL.SqlDAL sqlDAL = new Models.DAL.SqlDAL(dbConnection);
                Models.Repositories.Fighter.IFighterRepository fighterRepository = new Models.Repositories.Fighter.FighterRepository(sqlDAL);
                PlayerUoW uow = new PlayerUoW(sqlDAL);
                string newFighterId = uow.CreateFighter(fighter.Name, _userEmail, fighter.Picture);
            }

        }

        [HttpGet("GetPlayerBalance")]
        //public async Task<IEnumerable<Fighter>> PlayerFighters()
        public int GetPlayerBalance()
        {
            using (IDbConnection dbConnection =
            DbProviderFactories.GetFactory("system.data.sqlclient").CreateConnection())
            {
                dbConnection.ConnectionString = "Server=tcp:cipherrex.database.windows.net,1433;Initial Catalog=cipherRexUmbraco;Persist Security Info=False;User ID=cipherrex;Password=R00ksp@wnR00ksp@wn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                dbConnection.Open();
                Models.DAL.SqlDAL sqlDAL = new Models.DAL.SqlDAL(dbConnection);

                Models.Repositories.PlayerRepos.IPlayerRepo playerRepo = new Models.Repositories.PlayerRepos.PlayerRepo(sqlDAL);

                var _userEmail = userEmail().Result;

                return playerRepo.GetPlayer(_userEmail).Balance;

            }

        }

        [AllowAnonymous]
        private async Task<string> userEmail()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            //var user = await _userManager.FindByIdAsync(userId);
            //var userEmail = user.Email;
            //return userEmail;
            return "cipherRex@gmail.com";
        }

    }
}
