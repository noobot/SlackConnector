using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bazam.NoobWebClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;
using SlackConnector.EventHandlers;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using Group = SlackConnector.Connections.Handshaking.Models.Group;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IChatHubInterpreter _chatHubInterpreter;
        private readonly IMentionDetector _mentionDetector;
        private IWebSocketClient _webSocketClient;

        private const string SLACK_API_SEND_MESSAGE_URL = "https://slack.com/api/chat.postMessage";
        private const string SLACK_API_JOIN_DM_URL = "https://slack.com/api/im.open";

        //TODO: Remove?
        public string[] Aliases { get; set; } = new string[0];

        public SlackChatHub[] ConnectedDMs
        {
            get { return ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.DM).ToArray(); }
        }

        public SlackChatHub[] ConnectedChannels
        {
            get { return ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Channel).ToArray(); }
        }

        public SlackChatHub[] ConnectedGroups
        {
            get { return ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Group).ToArray(); }
        }

        private readonly Dictionary<string, SlackChatHub> _connectedHubs = new Dictionary<string, SlackChatHub>();
        public IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs => _connectedHubs;

        private readonly Dictionary<string, string> _userNameCache = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> UserNameCache => _userNameCache;

        public bool IsConnected => ConnectedSince.HasValue;
        public DateTime? ConnectedSince { get; private set; }
        public string SlackKey { get; private set; }
        public string TeamId { get; private set; }
        public string TeamName { get; private set; }
        public string UserId { get; private set; }
        public string UserName { get; private set; }

        public SlackConnector() : this(new ConnectionFactory(), new ChatHubInterpreter(), new MentionDetector())
        { }

        internal SlackConnector(IConnectionFactory connectionFactory, IChatHubInterpreter chatHubInterpreter, IMentionDetector mentionDetector)
        {
            _connectionFactory = connectionFactory;
            _chatHubInterpreter = chatHubInterpreter;
            _mentionDetector = mentionDetector;
        }

        public async Task Connect(string slackKey)
        {
            if (IsConnected)
            {
                throw new AlreadyConnectedException();
            }

            if (string.IsNullOrEmpty(slackKey))
            {
                throw new ArgumentNullException(nameof(slackKey));
            }

            SlackKey = slackKey;

            IHandshakeClient handshakeClient = _connectionFactory.CreateHandshakeClient();
            SlackHandshake handshake = await handshakeClient.FirmShake(slackKey);

            if (!handshake.Ok)
            {
                throw new HandshakeException(handshake.Error);
            }

            TeamName = handshake.Team.Name;
            TeamId = handshake.Team.Id;
            UserName = handshake.Self.Name;
            UserId = handshake.Self.Id;

            foreach (User user in handshake.Users)
            {
                _userNameCache.Add(user.Id, user.Name);
            }

            foreach (Channel channel in handshake.Channels.Where(x => !x.IsArchived))
            {
                var newChannel = new SlackChatHub
                {
                    Id = channel.Id,
                    Name = "#" + channel.Name,
                    Type = SlackChatHubType.Channel
                };
                _connectedHubs.Add(channel.Id, newChannel);
            }

            foreach (Group group in handshake.Groups.Where(x => !x.IsArchived))
            {
                if (group.Members.Any(x => x == UserId))
                {
                    var newGroup = new SlackChatHub
                    {
                        Id = group.Id,
                        Name = "#" + group.Name,
                        Type = SlackChatHubType.Group
                    };
                    _connectedHubs.Add(group.Id, newGroup);
                }
            }

            foreach (Im im in handshake.Ims)
            {
                var newIm = new SlackChatHub
                {
                    Id = im.Id,
                    Name = "@" + (_userNameCache.ContainsKey(im.User) ? _userNameCache[im.User] : im.User),
                    Type = SlackChatHubType.DM
                };
                _connectedHubs.Add(im.Id, newIm);
            }

            _webSocketClient = _connectionFactory.CreateWebSocketClient(handshake.WebSocketUrl);
            await _webSocketClient.Connect();

            ConnectedSince = DateTime.Now;
            RaiseConnectionStatusChanged();

            _webSocketClient.OnMessage += async (sender, message) => { await ListenTo(message); };
            _webSocketClient.OnClose += (sender, e) =>
            {
                ConnectedSince = null;
                RaiseConnectionStatusChanged();
            };
        }

        private async Task ListenTo(InboundMessage inboundMessage)
        {
            if (inboundMessage?.MessageType != MessageType.Message)
                return;
            if (string.IsNullOrEmpty(inboundMessage.User))
                return;
            if (!string.IsNullOrEmpty(UserId) && inboundMessage.User == UserId)
                return;

            if (inboundMessage.Channel != null && !_connectedHubs.ContainsKey(inboundMessage.Channel))
            {
                _connectedHubs[inboundMessage.Channel] = _chatHubInterpreter.FromId(inboundMessage.Channel);
            }

            var message = new SlackMessage
            {
                User = new SlackUser
                {
                    Id = inboundMessage.User,
                    Name = GetMessageUsername(inboundMessage),
                },
                Text = inboundMessage.Text,
                ChatHub = inboundMessage.Channel == null ? null : _connectedHubs[inboundMessage.Channel],
                RawData = inboundMessage.RawData,
                MentionsBot = _mentionDetector.WasBotMentioned(UserName, UserId, inboundMessage.Text)
            };

            try
            {
                await RaiseMessageReceived(message);
            }
            catch (Exception)
            {

            }
        }

        private string GetMessageUsername(InboundMessage inboundMessage)
        {
            string username = string.Empty;

            if (!string.IsNullOrEmpty(inboundMessage.User) && UserNameCache.ContainsKey(inboundMessage.User))
            {
                username = UserNameCache[inboundMessage.User];
            }

            return username;
        }

        public void Disconnect()
        {
            if (_webSocketClient != null && _webSocketClient.IsAlive)
            {
                _webSocketClient.Close();
            }
        }

        public async Task Say(BotMessage message)
        {
            string chatHubId = null;

            if (message.ChatHub != null)
            {
                chatHubId = message.ChatHub.Id;
            }

            if (!string.IsNullOrEmpty(chatHubId))
            {
                var values = new List<string>
                {
                    "token", this.SlackKey,
                    "channel", chatHubId,
                    "text", message.Text,
                    "as_user", "true"
                };

                if (message.Attachments.Count > 0)
                {
                    values.Add("attachments");
                    values.Add(JsonConvert.SerializeObject(message.Attachments));
                }

                var client = new NoobWebClient();
                await client.GetResponse(SLACK_API_SEND_MESSAGE_URL, RequestMethod.Post, values.ToArray());
            }
            else
            {
                throw new ArgumentException("When calling the Say() method, the message parameter must have its ChatHub property set.");
            }
        }

        public async Task<SlackChatHub> JoinDirectMessageChannel(string user)
        {
            SlackChatHub chatHub = null;

            var values = new[]
            {
                "token", this.SlackKey,
                "user", user
            };

            var client = new NoobWebClient();
            string json = await client.GetResponse(SLACK_API_JOIN_DM_URL, RequestMethod.Post, values);
            JObject jData = JObject.Parse(json);

            if (jData["ok"] != null && jData["ok"].Value<bool>())
            {
                chatHub = new SlackChatHub
                {
                    Id = jData["channel"]["id"].Value<string>(),
                    Type = SlackChatHubType.DM
                };
            }

            return chatHub;
        }


        public event ConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        private void RaiseConnectionStatusChanged()
        {
            OnConnectionStatusChanged?.Invoke(IsConnected);
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        private async Task RaiseMessageReceived(SlackMessage message)
        {
            if (OnMessageReceived != null)
            {
                await OnMessageReceived(message);
            }
        }
    }
}