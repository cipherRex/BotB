

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotB.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BotB.Server.Hubs
{
    public class ChatHub : Hub
    {

        private static readonly Dictionary<string, string> _userLookup = new Dictionary<string, string>();

        private List<string> allConnetionIdsBut(string recipientID)
        {
            return _userLookup.Where(x => x.Value != recipientID)
                     .Select(x => x.Key)
                     .ToList();
        }

        private List<string> allConnetionIdsBut(List<string> recipientIDs)
        {

            return _userLookup.Where(x => recipientIDs.All(p2 => p2 != x.Value))
                     .Select(x => x.Key)
                     .ToList();

        }

        public async Task SendAnimationsIdled(List<string> PlayerIds, string message)
        {
            await Clients.AllExcept(allConnetionIdsBut(PlayerIds)).SendAsync(Messages.ANIMATIONS_IDLED, message);
        }

        public async Task SendChallenge(string user, string recipient, string message)
        {
            await Clients.AllExcept(allConnetionIdsBut(recipient)).SendAsync(Messages.CHALLENGE, user, message);
        }

        public async Task SendCombatResult(List<string> PlayerIds, string message)
        {
            await Clients.AllExcept(allConnetionIdsBut(PlayerIds)).SendAsync(Messages.COMBAT_ROUND_RESULT, message);
        }

        public async Task AcceptChallenge(string user, string Player1Id, string Player2Id, string Fighter1Id, string Fighter2Id)
        {

            Dictionary<string, string> message1 = new Dictionary<string, string>();
            message1["FighterId"] = Fighter1Id;
            message1["PlayerId"] = Player1Id;
            message1["Role"] = "White";

            Dictionary<string, string> message2 = new Dictionary<string, string>();
            message2["FighterId"] = Fighter2Id;
            message2["PlayerId"] = Player2Id;
            message2["Role"] = "Black";

            await Clients.AllExcept(allConnetionIdsBut(Player1Id)).SendAsync(Messages.ACCEPT_CHALLENGE, user, System.Text.Json.JsonSerializer.Serialize(message1));
            await Clients.AllExcept(allConnetionIdsBut(Player2Id)).SendAsync(Messages.ACCEPT_CHALLENGE, user, System.Text.Json.JsonSerializer.Serialize(message2));

        }

        public async Task Register(string username)
        {


            var currentId = Context.ConnectionId;
            if (!_userLookup.ContainsKey(currentId))
            {
                _userLookup.Add(currentId, username);
                await Clients.AllExcept(currentId).SendAsync(
                        Messages.RECIEVE,
                        username, $"{username} joined the chat"
                    );
            }
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
