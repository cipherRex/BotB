
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
            CombatSession thisCombatSession = _combatManager.GetCombatSessionByFighterId(Move.FighterId);
            CombatResult result = thisCombatSession.AddMove(Move);

            if (result != null)
            {


                List<string> playerIds = new List<string>();
                playerIds.Add(thisCombatSession.Fighters.ToArray()[0].Value.ownerId);
                playerIds.Add(thisCombatSession.Fighters.ToArray()[1].Value.ownerId);
                await _chatHubContext.SendCombatResult(playerIds, System.Text.Json.JsonSerializer.Serialize(result));
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
