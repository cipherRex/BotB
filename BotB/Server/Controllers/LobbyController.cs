//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BotB.Server.Controllers
//{
//    public class LobbyController
//    {
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using BotB.Server.Models;
using BotB.Shared;
using BotB.Server.Hubs;
using BotB.Shared.CombatManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Data.Common;

namespace BotB.Server.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Arena _arena;
        ChatHub _chatHubContext;
        private readonly CombatManager _combatManager;

        public LobbyController(UserManager<ApplicationUser> userManager,
                                Arena arena,
                                Hubs.ChatHub chatHubContext,
                                CombatManager combatManager)
        {
            _chatHubContext = chatHubContext;
            _arena = arena;
            _userManager = userManager;
            _combatManager = combatManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Fighter>> Get()
        {

            // var _userEmail = userEmail().Result;
            return _arena.Fighters().ToArray();
        }


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

        [HttpGet("Combatants")]
        public async Task<IEnumerable<Fighter>> Combatants()
        {
            return _arena.Fighters().ToArray();
        }

        [HttpPost("EnterLobby")]
        public async Task EnterLobby([FromBody] string FigherJson)
        {


            var _userEmail = userEmail().Result;
            Fighter newFighter = System.Text.Json.JsonSerializer.Deserialize<Fighter>(FigherJson);
            newFighter.ownerEmail = _userEmail;
            _arena.AddFighter(newFighter);
            await _chatHubContext.Clients.All.SendAsync(Messages.ENTER_LOBBY, _userEmail, JsonSerializer.Serialize(newFighter));

        }

        [HttpPost("ExitLobby")]
        public async Task ExitLobby([FromBody] string FigherJson)
        {
            var _userEmail = userEmail().Result;
            Fighter fighterToRemove = System.Text.Json.JsonSerializer.Deserialize<Fighter>(FigherJson);

            Fighter fx = _arena.Fighters().First(x => x.id == fighterToRemove.id);

            _arena.RemoveFighter(fx);
            await _chatHubContext.Clients.All.SendAsync(Messages.LEAVE_LOBBY, _userEmail, JsonSerializer.Serialize(fighterToRemove));

        }

        [HttpPost("Challenge")]
        public async Task Challenge([FromBody] string FigherJson)
        {
            var _userEmail = userEmail().Result;
            Fighter challengedFighter = System.Text.Json.JsonSerializer.Deserialize<Fighter>(FigherJson);

            string myFighterId = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(FigherJson).ChallengerFighterId;

            Fighter myFighter = _arena.Fighters().Where(x => x.id == myFighterId).First();

            await _chatHubContext.SendChallenge(_userEmail, challengedFighter.ownerId, System.Text.Json.JsonSerializer.Serialize(myFighter));

        }

        [HttpPost("AcceptChallenge")]
        public async Task AcceptChallenge([FromBody] string FigherJson)
        {
            var _userEmail = userEmail().Result;
            Fighter challengedFighter = System.Text.Json.JsonSerializer.Deserialize<Fighter>(FigherJson);

            string myFighterId = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(FigherJson).ChallengerFighterId;

            Fighter myFighter = _arena.Fighters().Where(x => x.id == myFighterId).First();


            //string sessionId = _combatManager.AddCombatSession(new CombatSession(myFighter.ownerId, challengedFighter.ownerId));
            _combatManager.CreateCombatSession(myFighter.id, challengedFighter.id);

            await _chatHubContext.AcceptChallenge(_userEmail, myFighter.ownerId, challengedFighter.ownerId, myFighter.id, challengedFighter.id);

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
