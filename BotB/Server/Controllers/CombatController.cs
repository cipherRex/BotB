
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotB.Shared;
using BotB.Shared.CombatManagement;
using BotB.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BotB.Server.Models.DAL;
using System.Data;
using System.Data.Common;
using BotB.Server.Models.UoW;
using BotB.Server.Models.Repositories.PlayerRepos;

namespace BotB.Server.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class CombatController : ControllerBase
    {
        private readonly CombatManager _combatManager;
        private readonly Hubs.ChatHub _chatHubContext;

        public CombatController(CombatManager combatManager, Hubs.ChatHub chatHubContext)
        {
            _combatManager = combatManager;
            _chatHubContext = chatHubContext;
            ;

        }

        [HttpPost("CombatMove")]
        public async void CombatMove([FromBody] CombatMove Move)
        {
            CombatSession thisCombatSession = _combatManager.GetCombatSessionByFighterId(Move.FighterId);
            CombatResult result = thisCombatSession.AddMove(Move);
            //CombatResult result = _combatManager.AddMove(Move);

            if (result != null)
            {
                List<string> playerIds = new List<string>();
                playerIds.Add(thisCombatSession.Fighters.ToArray()[0].Value.ownerId);
                playerIds.Add(thisCombatSession.Fighters.ToArray()[1].Value.ownerId);
                //await _chatHubContext.SendCombatResult(playerIds, System.Text.Json.JsonSerializer.Serialize(result));
                 _chatHubContext.SendCombatResult(playerIds, System.Text.Json.JsonSerializer.Serialize(result));

                //IDAL dal = new SqlDAL();

                if (result.Victory != null) 
                {
                    _combatManager.DeleteSession(playerIds[0]);

                    using (IDbConnection dbConnection =
                          DbProviderFactories.GetFactory("system.data.sqlclient").CreateConnection())
                    {
                        dbConnection.ConnectionString = "Server=tcp:cipherrex.database.windows.net,1433;Initial Catalog=cipherRexUmbraco;Persist Security Info=False;User ID=cipherrex;Password=R00ksp@wnR00ksp@wn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                        dbConnection.Open();
                        Models.DAL.SqlDAL sqlDAL = new Models.DAL.SqlDAL(dbConnection);

                        IPlayerRepo playerRepo = new PlayerRepo(sqlDAL);
                        PlayerUoW playerUoW = new PlayerUoW(sqlDAL, playerRepo);

                        playerUoW.EndGameAsync(
                            result.Victory.VictorFighterId,
                            thisCombatSession.Fighters.Where(x => x.Key != result.Victory.VictorFighterId).FirstOrDefault().Key 
                            );

                    }

                    

                }
            }
        }


        [HttpPost("AnimationIdled")]
        public async void AnimationIdled([FromBody] string fighterId)
        {
            if (_combatManager.setSemaphore(fighterId))
            {
                //CombatSession thisCombatSession = _combatManager.GetCombatSessionByFighterId(fighterId);
                //List<string> keys = thisCombatSession.AnimationSemaphore.Keys.ToList<string>();
                
                //List<string> playerIds = new List<string>();

                //playerIds.AddRange
                //    (
                //        _combatManager.GetCombatSessionByFighterId(fighterId).Fighters.Select(x => x.Value.ownerId)
                //    );

                //await _chatHubContext.SendAnimationsIdled(playerIds, "ok");
                await _chatHubContext.SendAnimationsIdled(
                    _combatManager.GetCombatSessionByFighterId(fighterId).Fighters.Select(x => x.Value.ownerId).ToList()
                    , "ok");

            }
        }
    }
}
