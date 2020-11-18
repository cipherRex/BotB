//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BotB.Client.Data
//{
//    public class ChatClient
//    {
//    }
//}

using BotB.Shared;
using Microsoft.AspNetCore.Components;
//requires Microsoft.AspNetCore.SignalR.Client nuget:
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Client.Data
{
    public class ChatClient : IAsyncDisposable
    {
        public const string HUBURL = "/ChatHub";
        private readonly NavigationManager _navigationManager;
        private HubConnection _hubConnection;
        private readonly string _username;
        private bool _started = false;

        public ChatClient(string username, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _username = username;
        }

        public async Task StartAsync()
        {
            if (!_started)
            {

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_navigationManager.ToAbsoluteUri(HUBURL))
                    .Build();

                //  Console.WriteLine("ChatClient:calling Start()");

                _hubConnection.On<string, string>(Messages.ENTER_LOBBY, (user, message) =>
                {
                    HandleReceiveMessage(_username, message);
                });

                _hubConnection.On<string, string>(Messages.LEAVE_LOBBY, (user, message) =>
                {
                    HandleLobbyExitedMessage(_username, message);
                });

                _hubConnection.On<string, string>(Messages.CHALLENGE, (user, message) =>
                {
                    HandleChallenge(_username, message);
                });

                _hubConnection.On<string, string>(Messages.ACCEPT_CHALLENGE, (user, message) =>
                {
                    HandleChallengeAccepted(_username, message);
                });

                _hubConnection.On<string>(Messages.COMBAT_ROUND_RESULT, (message) =>
                {
                    HandleCombatRoundResult(message);

                });

                _hubConnection.On<string>(Messages.ANIMATIONS_IDLED, (message) =>
                {
                    HandleAnimationsIdled(message);

                });

                await _hubConnection.StartAsync();

                //Console.WriteLine("ChatClient:calling Start Returned");
                _started = true;

                await _hubConnection.SendAsync(Messages.REGISTER, _username);

            }
        }

        private void HandleReceiveMessage(string username, string message)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(username, message));
        }
        public event MessageReceivedEventHandler MessageReceived;
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);


        private void HandleLobbyExitedMessage(string username, string message)
        {
            LobbyExited?.Invoke(this, new LobbyExitedEventArgs(username, message));
        }
        public event LobbyExitedEventHandler LobbyExited;
        public delegate void LobbyExitedEventHandler(object sender, LobbyExitedEventArgs e);

        private void HandleChallenge(string username, string message)
        {
            Challenged?.Invoke(this, new ChallengedEventArgs(username, message));
        }
        public event ChallengedEventHandler Challenged;
        public delegate void ChallengedEventHandler(object sender, ChallengedEventArgs e);

        private void HandleChallengeAccepted(string username, string message)
        {
            ChallengeAccepted?.Invoke(this, new ChallengeAcceptedEventArgs(username, message));
        }
        public event ChallengeAcceptedEventHandler ChallengeAccepted;
        public delegate void ChallengeAcceptedEventHandler(object sender, ChallengeAcceptedEventArgs e);

        //private void HandleCombatRoundResult(string username, string message)
        //private void HandleCombatRoundResult(string message)
        private void HandleCombatRoundResult(string message)
        {
            //CombatRoundResult?.Invoke(this, new CombatRoundResultEventArgs("username", message));
            CombatRoundResult?.Invoke(this, new CombatRoundResultEventArgs(message));
        }
        public event CombatRoundResultEventHandler CombatRoundResult;
        public delegate void CombatRoundResultEventHandler(object sender, CombatRoundResultEventArgs e);

        private void HandleAnimationsIdled(string message)
        {
            //CombatRoundResult?.Invoke(this, new CombatRoundResultEventArgs("username", message));
            HandleAnimationsIdledResult?.Invoke(this, new CombatRoundResultEventArgs(message));
        }
        public event HandleAnimationsIdledResultEventHandler HandleAnimationsIdledResult;
        public delegate void HandleAnimationsIdledResultEventHandler(object sender, CombatRoundResultEventArgs e);



        public async Task SendAsync(string message)
        {
            if (!_started)
            {
                throw new InvalidOperationException("Client not started");
            }
            await _hubConnection.SendAsync(Messages.SEND, _username, message);

        }

        public async Task StopAsync()
        {
            if (_started)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
                _started = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            //   Console.WriteLine("ChatClient:Disposing");
            await StopAsync();
        }

    }

    //public class MessageReceivedEventArgs : EventArgs
    //{
    //    public string Username { get; set; }
    //    public string Message { get; set; }

    //    public MessageReceivedEventArgs(string username, string message)
    //    {
    //        Username = username;
    //        Message = message;
    //    }

    //}

    public class MessageReceivedEventArgs : EventArgs
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }

        public MessageReceivedEventArgs(string messageType, string messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }

    }

    public class LobbyExitedEventArgs : EventArgs
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }

        public LobbyExitedEventArgs(string messageType, string messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }

    }

    public class ChallengedEventArgs : EventArgs
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }

        public ChallengedEventArgs(string messageType, string messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }

    }

    public class ChallengeAcceptedEventArgs : EventArgs
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }

        public ChallengeAcceptedEventArgs(string messageType, string messageContent)
        {
            MessageType = messageType;
            MessageContent = messageContent;
        }

    }

    public class CombatRoundResultEventArgs : EventArgs
    {
        //public string MessageType { get; set; }
        public string MessageContent { get; set; }

        //public CombatRoundResultEventArgs(string messageType, string messageContent)
        public CombatRoundResultEventArgs(string messageContent)
        {
            //MessageType = messageType;
            MessageContent = messageContent;
        }

    }
}
