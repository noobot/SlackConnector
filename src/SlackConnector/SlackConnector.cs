using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bazam.NoobWebClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackConnector.BotHelpers;
using SlackConnector.EventHandlers;
using SlackConnector.Models;
using WebSocketSharp;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        private const string SLACK_API_START_URL = "https://slack.com/api/rtm.start";
        private const string SLACK_API_SEND_MESSAGE_URL = "https://slack.com/api/chat.postMessage";
        private readonly Dictionary<string, string> _userNameCache;
        private WebSocket _webSocket;

        public string[] Aliases { get; set; }

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

        public IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; private set; }
        public bool IsConnected => ConnectedSince != null;
        public DateTime? ConnectedSince { get; private set; }
        public string SlackKey { get; private set; }
        public string TeamId { get; private set; }
        public string TeamName { get; private set; }
        public string UserId { get; private set; }
        public string UserName { get; private set; }

        public SlackConnector()
        {
            Aliases = new string[0];
            _userNameCache = new Dictionary<string, string>();
            ConnectedHubs = new Dictionary<string, SlackChatHub>();
        }

        public async Task Connect(string slackKey)
        {
            this.SlackKey = slackKey;

            // disconnect in case we're already connected like a crazy person
            Disconnect();
            
            NoobWebClient client = new NoobWebClient();
            string json = await client.GetResponse(SLACK_API_START_URL, RequestMethod.Post, "token", this.SlackKey);
            JObject jData = JObject.Parse(json);

            TeamId = jData["team"]["id"].Value<string>();
            TeamName = jData["team"]["name"].Value<string>();
            UserId = jData["self"]["id"].Value<string>();
            UserName = jData["self"]["name"].Value<string>();
            string webSocketUrl = jData["url"].Value<string>();

            _userNameCache.Clear();
            foreach (JObject userObject in jData["users"])
            {
                _userNameCache.Add(userObject["id"].Value<string>(), userObject["name"].Value<string>());
            }

            // load the channels, groups, and DMs that margie's in
            Dictionary<string, SlackChatHub> hubs = new Dictionary<string, SlackChatHub>();
            ConnectedHubs = hubs;

            // channelz
            if (jData["channels"] != null)
            {
                foreach (JObject channelData in jData["channels"])
                {
                    if (!channelData["is_archived"].Value<bool>() && channelData["is_member"].Value<bool>())
                    {
                        SlackChatHub channel = new SlackChatHub()
                        {
                            Id = channelData["id"].Value<string>(),
                            Name = "#" + channelData["name"].Value<string>(),
                            Type = SlackChatHubType.Channel
                        };
                        hubs.Add(channel.Id, channel);
                    }
                }
            }

            // groupz
            if (jData["groups"] != null)
            {
                foreach (JObject groupData in jData["groups"])
                {
                    if (!groupData["is_archived"].Value<bool>() && groupData["members"].Values<string>().Contains(UserId))
                    {
                        SlackChatHub group = new SlackChatHub()
                        {
                            Id = groupData["id"].Value<string>(),
                            Name = groupData["name"].Value<string>(),
                            Type = SlackChatHubType.Group
                        };
                        hubs.Add(group.Id, group);
                    }
                }
            }

            // dmz
            if (jData["ims"] != null)
            {
                foreach (JObject dmData in jData["ims"])
                {
                    string userID = dmData["user"].Value<string>();
                    SlackChatHub dm = new SlackChatHub()
                    {
                        Id = dmData["id"].Value<string>(),
                        Name = "@" + (_userNameCache.ContainsKey(userID) ? _userNameCache[userID] : userID),
                        Type = SlackChatHubType.DM
                    };
                    hubs.Add(dm.Id, dm);
                }
            }

            // set up the websocket and connect
            _webSocket = new WebSocket(webSocketUrl);

            _webSocket.OnOpen += (object sender, EventArgs e) =>
            {
                // set connection-related properties
                ConnectedSince = DateTime.Now;
                RaiseConnectionStatusChanged();
            };

            _webSocket.OnMessage += async (object sender, MessageEventArgs args) =>
            {
                await ListenTo(args.Data);
            };

            _webSocket.OnClose += (object sender, CloseEventArgs e) =>
            {
                // set connection-related properties
                ConnectedSince = null;
                TeamId = null;
                TeamName = null;
                UserId = null;
                UserName = null;
                RaiseConnectionStatusChanged();
            };
            _webSocket.Connect();
        }

        private async Task ListenTo(string json)
        {
            JObject jObject = JObject.Parse(json);
            if (jObject["type"].Value<string>() == "message")
            {
                string channelId = jObject["channel"].Value<string>();
                SlackChatHub hub;

                if (ConnectedHubs.ContainsKey(channelId))
                {
                    hub = ConnectedHubs[channelId];
                }
                else
                {
                    hub = SlackChatHub.FromID(channelId);
                    List<SlackChatHub> hubs = new List<SlackChatHub>();
                    hubs.AddRange(ConnectedHubs.Values);
                    hubs.Add(hub);
                }

                string messageText = (jObject["text"] != null ? jObject["text"].Value<string>() : null);

                // check to see if bot has been mentioned
                var message = new SlackMessage
                {
                    ChatHub = hub,
                    MentionsBot = BotMentioned(messageText),
                    RawData = json,
                    // some messages may not have text or a user (like unfurled data from URLs)
                    Text = messageText,
                    User = (jObject["user"] != null ? new SlackUser { Id = jObject["user"].Value<string>() } : null)
                };

                var context = new ResponseContext
                {
                    BotHasResponded = false,
                    BotUserID = UserId,
                    BotUserName = UserName,
                    Message = message,
                    TeamId = TeamId,
                    UserNameCache = new ReadOnlyDictionary<string, string>(_userNameCache)
                };

                // margie can never respond to herself and requires that the message have text and be from an actual person
                if (message.User != null && message.User.Id != UserId && message.Text != null)
                {
                    await RaiseMessageReceived(context);
                }
            }
        }

        public void Disconnect()
        {
            if (_webSocket != null && _webSocket.IsAlive)
            {
                _webSocket.Close();
            }
        }

        public async Task Say(BotMessage message)
        {
            string chatHubId = null;

            if (message.ChatHub != null)
            {
                chatHubId = message.ChatHub.Id;
            }

            if (chatHubId != null)
            {
                var client = new NoobWebClient();

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

                await client.GetResponse(SLACK_API_SEND_MESSAGE_URL, RequestMethod.Post, values.ToArray());
            }
            else
            {
                throw new ArgumentException("When calling the Say() method, the message parameter must have its ChatHub property set.");
            }
        }

        private bool BotMentioned(string messageText)
        {
            bool mentioned = false;

            // only build the regex if we're connected - if we're not connected we won't know our bot's name or user Id
            if (IsConnected)
            {
                string regex = new BotNameRegexComposer().ComposeFor(UserName, UserId, Aliases);
                mentioned = (messageText != null && Regex.IsMatch(messageText, regex, RegexOptions.IgnoreCase));
            }

            return mentioned;
        }

        public event ConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        private void RaiseConnectionStatusChanged()
        {
            if (OnConnectionStatusChanged != null)
            {
                OnConnectionStatusChanged(IsConnected);
            }
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        private async Task RaiseMessageReceived(ResponseContext responseContext)
        {
            if (OnMessageReceived != null)
            {
                await OnMessageReceived(responseContext);
            }
        }
    }
}