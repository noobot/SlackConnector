using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using SlackLibrary.BotHelpers;
using SlackLibrary.Connections;
using SlackLibrary.Connections.Clients.Channel;
using SlackLibrary.Connections.Models;
using SlackLibrary.Connections.Monitoring;
using SlackLibrary.Connections.Sockets;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Connections.Sockets.Messages.Outbound;
using SlackLibrary.EventHandlers;
using SlackLibrary.Exceptions;
using SlackLibrary.Extensions;
using SlackLibrary.Models;
using SlackLibrary.Connections.Sockets.Messages.Inbound.ReactionItem;
using SlackLibrary.Models.Reactions;

namespace SlackLibrary
{
    internal partial class SlackConnection : ISlackConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IMentionDetector _mentionDetector;
        private readonly IMonitoringFactory _monitoringFactory;
        private IWebSocketClient _webSocketClient;
        private IPingPongMonitor _pingPongMonitor;

        private Dictionary<string, SlackChatHub> _connectedHubs { get; set; }
        public IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs => _connectedHubs;

        private Dictionary<string, SlackUser> _userCache { get; set; }
        public IReadOnlyDictionary<string, SlackUser> UserCache => _userCache;

        public bool IsConnected => _webSocketClient?.IsAlive ?? false;
        public DateTime? ConnectedSince { get; private set; }
        public string SlackKey { get; private set; }

        public ContactDetails Team { get; private set; }
        public ContactDetails Self { get; private set; }

        public SlackConnection(IConnectionFactory connectionFactory, IMentionDetector mentionDetector, IMonitoringFactory monitoringFactory)
        {
            _connectionFactory = connectionFactory;
            _mentionDetector = mentionDetector;
            _monitoringFactory = monitoringFactory;
        }

        public async Task Initialise(ConnectionInformation connectionInformation)
        {
            SlackKey = connectionInformation.SlackKey;
            Team = connectionInformation.Team;
            Self = connectionInformation.Self;
            _userCache = connectionInformation.Users;
            _connectedHubs = connectionInformation.SlackChatHubs;

            _webSocketClient = connectionInformation.WebSocket;
            _webSocketClient.OnClose += (sender, args) =>
            {
                ConnectedSince = null;
                RaiseOnDisconnect();
            };

            _webSocketClient.OnMessage += async (sender, message) => await ListenTo(message);

            ConnectedSince = DateTime.Now;

            _pingPongMonitor = _monitoringFactory.CreatePingPongMonitor();
            await _pingPongMonitor.StartMonitor(Ping, Reconnect, TimeSpan.FromMinutes(2));
        }

        private async Task Reconnect()
        {
            var reconnectingEvent = RaiseOnReconnecting();

            var handshakeClient = _connectionFactory.CreateHandshakeClient();
            var handshake = await handshakeClient.FirmShake(SlackKey);
            await _webSocketClient.Connect(handshake.WebSocketUrl);

            await Task.WhenAll(reconnectingEvent, RaiseOnReconnect());
        }

        private SlackUser GetMessageUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return UserCache.ContainsKey(userId)
                ? UserCache[userId]
                : new SlackUser { Id = userId, Name = string.Empty };
        }

        private SlackChatHub GetChatHub(string channel)
        {
            return channel != null && _connectedHubs.ContainsKey(channel)
                ? _connectedHubs[channel]
                : null;
        }

        public async Task Close()
        {
            if (_webSocketClient != null && _webSocketClient.IsAlive)
            {
                await _webSocketClient.Close();
            }
        }

        public async Task Say(BotMessage message)
        {
            if (string.IsNullOrEmpty(message.ChatHub?.Id))
            {
                throw new MissingChannelException("When calling the Say() method, the message parameter must have its ChatHub property set.");
            }

            var client = _connectionFactory.CreateChatClient();
            await client.PostMessage(SlackKey, message.ChatHub.Id, message.Text, message.Attachments,
				message.ThreadTimestamp, message.IconUrl, message.UserName);
        }

        public async Task Upload(SlackChatHub chatHub, string filePath)
        {
            var client = _connectionFactory.CreateFileClient();
            await client.PostFile(SlackKey, chatHub.Id, filePath);
        }

        public async Task Upload(SlackChatHub chatHub, Stream stream, string fileName)
        {
            var client = _connectionFactory.CreateFileClient();
            await client.PostFile(SlackKey, chatHub.Id, stream, fileName);
        }

        public async Task<IEnumerable<SlackChatHub>> GetChannels()
        {
            IChannelClient client = _connectionFactory.CreateChannelClient();

            var channelsTask = client.GetChannels(SlackKey);
            var groupsTask = client.GetGroups(SlackKey);
            await Task.WhenAll(channelsTask, groupsTask);

            var fromChannels = channelsTask.Result.Select(c => c.ToChatHub());
            var fromGroups = groupsTask.Result.Select(g => g.ToChatHub());
            return fromChannels.Concat(fromGroups);
        }

        public async Task<IEnumerable<SlackUser>> GetUsers()
        {
            var client = _connectionFactory.CreateUserClient();
            var users = await client.ListAll(SlackKey);

            //TODO: Update user cache
            return users.Select(u => u.ToSlackUser());
        }

        //TODO: Cache newly created channel, and return if already exists
        public async Task<SlackChatHub> JoinDirectMessageChannel(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentNullException(nameof(user));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            Channel channel = await client.JoinDirectMessageChannel(SlackKey, user);

            return new SlackChatHub
            {
                Id = channel.Id,
                Name = channel.Name,
                Type = SlackChatHubType.DM
            };
        }

        public async Task<SlackChatHub> JoinChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            Channel channel = await client.JoinChannel(SlackKey, channelName);

            return new SlackChatHub
            {
                Id = channel.Id,
                Name = channel.Name,
                Type = SlackChatHubType.Channel
            };
        }

        public async Task<SlackChatHub> CreateChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            Channel channel = await client.CreateChannel(SlackKey, channelName);

            return new SlackChatHub
            {
                Id = channel.Id,
                Name = channel.Name,
                Type = SlackChatHubType.Channel
            };
        }

        public async Task ArchiveChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            await client.ArchiveChannel(SlackKey, channelName);
        }

        public async Task<SlackPurpose> SetChannelPurpose(string channelName, string purpose)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            if (string.IsNullOrEmpty(purpose))
            {
                throw new ArgumentNullException(nameof(purpose));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            string purposeSet = await client.SetPurpose(SlackKey, channelName, purpose);

            return new SlackPurpose
            {
                ChannelName = channelName,
                Purpose = purposeSet
            };
        }

        public async Task<SlackTopic> SetChannelTopic(string channelName, string topic)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }

            IChannelClient client = _connectionFactory.CreateChannelClient();
            string topicSet = await client.SetTopic(SlackKey, channelName, topic);

            return new SlackTopic
            {
                ChannelName = channelName,
                Topic = topicSet
            };
        }

        public async Task IndicateTyping(SlackChatHub chatHub)
        {
            var message = new TypingIndicatorMessage
            {
                Channel = chatHub.Id
            };

            await _webSocketClient.SendMessage(message);
        }

        public async Task SubscribeToPresence(IEnumerable<string> ids)
        {
            var message = new PresenceSubMessage
            {
                Ids = ids
            };

            await _webSocketClient.SendMessage(message);
        }

        public async Task QueryPresence(IEnumerable<string> ids)
        {
            var message = new PresenceQueryMessage
            {
                Ids = ids
            };

            await _webSocketClient.SendMessage(message);
        }

        public async Task Ping()
        {
            await _webSocketClient.SendMessage(new PingMessage());
        }

		public Task<Stream> DownloadFile(Uri downloadUri)
		{
			if (!downloadUri.Host.Equals("files.slack.com"))
			{
				throw new ArgumentException("Invalid uri. Should be targetting files.slack.com", nameof(downloadUri));
			}

			return downloadUri.AbsoluteUri
				.WithOAuthBearerToken(SlackKey)
				.AllowHttpStatus()
				.GetStreamAsync();
		}

		public event DisconnectEventHandler OnDisconnect;
        private void RaiseOnDisconnect()
        {
            OnDisconnect?.Invoke();
        }

        public event ReconnectEventHandler OnReconnecting;
        private async Task RaiseOnReconnecting()
        {
            var e = OnReconnecting;
            if (e != null)
            {
                try
                {
                    await e();
                }
                catch (Exception)
                {

                }
            }
        }

        public event ReconnectEventHandler OnReconnect;
        private async Task RaiseOnReconnect()
        {
            var e = OnReconnect;
            if (e != null)
            {
                try
                {
                    await e();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
