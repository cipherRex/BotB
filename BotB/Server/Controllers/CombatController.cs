
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotB.Shared;
using BotB.Shared.CombatManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace BotB.Server.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class CombatController : ControllerBase
    {
        private readonly BotB.Shared.CombatManagement.CombatManager _combatManager;
        private readonly Hubs.ChatHub _chatHubContext;

        public CombatController(BotB.Shared.CombatManagement.CombatManager combatManager, Hubs.ChatHub chatHubContext)
        {
            _combatManager = combatManager;
            _chatHubContext = chatHubContext;
            ;

        }

        [HttpPost("CombatMove")]
        public async void CombatMove([FromBody] CombatMove Move)
        {
            string x = "";

            //CombatResult result = _combatManager.Sessions[Move.SessionId].Resolve(Move);

            //if (result != null)
            //{
            //    List<string> playerIds = new List<string>();
            //    playerIds.Add(_combatManager.Sessions[Move.SessionId].Player1Id);
            //    playerIds.Add(_combatManager.Sessions[Move.SessionId].Player2Id);

            //    //_combatManager.Sessions[Move.SessionId].
            //    Console.WriteLine("WhitePlayerTotalHP: " + result.WhitePlayerTotalHP.ToString());
            //    Console.WriteLine("BlackPlayerTotalHP: " + result.BlackPlayerTotalHP.ToString());

            //    System.Diagnostics.Debug.WriteLine("white hp: " + result.WhitePlayerTotalHP);
            //    System.Diagnostics.Debug.WriteLine("black hp: " + result.BlackPlayerTotalHP);


            //    if (result.BlackPlayerTotalHP < 0)
            //    {
            //        result.VictoryData = new Victory() { FighterId = _combatManager.Sessions[Move.SessionId].WhiKnightID, Condition = "DIE" };
            //    }
            //    else if (result.WhitePlayerTotalHP < 0)
            //    {
            //        result.VictoryData = new Victory() { FighterId = _combatManager.Sessions[Move.SessionId].BlackKnightID, Condition = "DIE" };
            //    }

            //    await _chatHubContext.SendCombatResult(playerIds, System.Text.Json.JsonSerializer.Serialize(result));


        }


        [HttpPost("AnimationIdled")]
        public async void AnimationIdled([FromBody] string fighterIdAndSessionId)
        {

            string x = "";

            //var tuple = Newtonsoft.Json.JsonConvert.DeserializeObject<Tuple<string, string>>(fighterIdAndSessionId);

            //string fighterId = tuple.Item1;
            //string sessionId = tuple.Item2;

            ////flip the semaphore for this fighter, indicating it has reentered idle and;
            //_combatManager.Sessions[sessionId].AnimationSemaphore[fighterId] = true;

            ////check to see if both fighters are now idled. if so, clear out both semaphores and send out signaR notification
            //if (_combatManager.Sessions[sessionId].AnimationSemaphore.Where(x => x.Value).Count() == 2)
            //{
            //    List<string> keys = _combatManager.Sessions[sessionId].AnimationSemaphore.Keys.ToList<string>();

            //    foreach (string key in keys)
            //    {
            //        if (_combatManager.Sessions[sessionId].AnimationSemaphore[key])
            //        {
            //            _combatManager.Sessions[sessionId].AnimationSemaphore[key] = false;
            //        }
            //    }

            //    List<string> playerIds = new List<string>();
            //    playerIds.Add(_combatManager.Sessions[sessionId].Player1Id);
            //    playerIds.Add(_combatManager.Sessions[sessionId].Player2Id);
            //    await _chatHubContext.SendAnimationsIdled(playerIds, "ok");

            //}
        }


    }

}
