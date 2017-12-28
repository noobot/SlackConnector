using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.EventHandlers;
using SlackConnector.Exceptions;
using SlackConnector.Extensions;
using SlackConnector.Models;
using SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem;
using SlackConnector.Models.Reactions;

namespace SlackConnector
{
    internal class SlackConnection : ISlackConnection
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

        private Task ListenTo(InboundMessage inboundMessage)
        {
            if (inboundMessage == null)
            {
                return Task.CompletedTask;
            }

            //TODO: Visitor pattern?
            switch (inboundMessage.MessageType)
            {
                case MessageType.Message: return HandleMessage((ChatMessage)inboundMessage);
                case MessageType.Group_Joined: return HandleGroupJoined((GroupJoinedMessage)inboundMessage);
                case MessageType.Channel_Joined: return HandleChannelJoined((ChannelJoinedMessage)inboundMessage);
                case MessageType.Im_Created: return HandleDmJoined((DmChannelJoinedMessage)inboundMessage);
                case MessageType.Team_Join: return HandleUserJoined((UserJoinedMessage)inboundMessage);
                case MessageType.Pong: return HandlePong((PongMessage)inboundMessage);
                case MessageType.Reaction_Added: return HandleReaction((ReactionMessage)inboundMessage);
                case MessageType.Channel_Created: return HandleChannelCreated((ChannelCreatedMessage)inboundMessage);
            }

            return Task.CompletedTask;
        }

        private Task HandleMessage(ChatMessage inboundMessage)
        {
            if (string.IsNullOrEmpty(inboundMessage.User))
                return Task.CompletedTask;

            if (!string.IsNullOrEmpty(Self.Id) && inboundMessage.User == Self.Id)
                return Task.CompletedTask;

            //TODO: Insert into connectedHubs when DM is missing

            var message = new SlackMessage
            {
                User = GetMessageUser(inboundMessage.User),
                Timestamp = inboundMessage.Timestamp,
                Text = inboundMessage.Text,
                ChatHub = GetChatHub(inboundMessage.Channel),
                RawData = inboundMessage.RawData,
                MentionsBot = _mentionDetector.WasBotMentioned(Self.Name, Self.Id, inboundMessage.Text),
                MessageSubType = inboundMessage.MessageSubType.ToSlackMessageSubType()
            };

            return RaiseMessageReceived(message);
        }

        private Task HandleReaction(ReactionMessage inboundMessage)
        {
            if (string.IsNullOrEmpty(inboundMessage.User))
                return Task.CompletedTask;

            if (!string.IsNullOrEmpty(Self.Id) && inboundMessage.User == Self.Id)
                return Task.CompletedTask;

            if (inboundMessage.ReactingTo is MessageReaction messageReaction)
            {
                //TODO: Interface methods? Extension methods?
                return RaiseReactionReceived(
                    new SlackMessageReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        ChatHub = GetChatHub(messageReaction.Channel),
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            if (inboundMessage.ReactingTo is FileReaction fileReaction)
            {
                return RaiseReactionReceived(
                    new SlackFileReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        File = fileReaction.File,
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            if (inboundMessage.ReactingTo is FileCommentReaction fileCommentReaction)
            {
                return RaiseReactionReceived(
                    new SlackFileCommentReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        File = fileCommentReaction.File,
                        FileComment = fileCommentReaction.FileComment,
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            return RaiseReactionReceived(
                new SlackUnknownReaction
                {
                    User = GetMessageUser(inboundMessage.User),
                    Timestamp = inboundMessage.Timestamp,
                    RawData = inboundMessage.RawData,
                    Reaction = inboundMessage.Reaction,
                    ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                });
        }

        private Task HandleGroupJoined(GroupJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleChannelJoined(ChannelJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleDmJoined(DmChannelJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub(_userCache.Values.ToArray());
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleUserJoined(UserJoinedMessage inboundMessage)
        {
            SlackUser slackUser = inboundMessage.User.ToSlackUser();
            _userCache[slackUser.Id] = slackUser;

            return RaiseUserJoined(slackUser);
        }

        private Task HandlePong(PongMessage inboundMessage)
        {
            _pingPongMonitor.Pong();
            return RaisePong(inboundMessage.Timestamp);
        }

        private Task HandleChannelCreated(ChannelCreatedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;
            
            var slackChannelCreated = new SlackChannelCreated
            {
                Id = channelId,
                Name = inboundMessage.Channel.Name,
                Creator = GetMessageUser(inboundMessage.Channel.Creator)
            };
            return RaiseOnChannelCreated(slackChannelCreated);
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
            await client.PostMessage(SlackKey, message.ChatHub.Id, message.Text, message.Attachments);
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
            IChannelClient client = _connectionFactory.CreateChannelClient();
            var users = await client.GetUsers(SlackKey);

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

        public async Task Ping()
        {
            await _webSocketClient.SendMessage(new PingMessage());
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

        public event MessageReceivedEventHandler OnMessageReceived;
        private async Task RaiseMessageReceived(SlackMessage message)
        {
            var e = OnMessageReceived;
            if (e != null)
            {
                try
                {
                    await e(message);
                }
                catch (Exception)
                {

                }
            }
        }

        public event ReactionReceivedEventHandler OnReaction;
        private async Task RaiseReactionReceived(ISlackReaction reaction)
        {
            var e = OnReaction;
            if (e != null)
            {
                try
                {
                    await e(reaction);
                }
                catch (Exception)
                {

                }
            }
        }

        public event ChatHubJoinedEventHandler OnChatHubJoined;
        private async Task RaiseChatHubJoined(SlackChatHub hub)
        {
            var e = OnChatHubJoined;
            if (e != null)
            {
                try
                {
                    await e(hub);
                }
                catch
                {
                }
            }
        }

        public event UserJoinedEventHandler OnUserJoined;
        private async Task RaiseUserJoined(SlackUser user)
        {
            var e = OnUserJoined;
            try
            {
                if (e != null)
                {
                    await e(user);
                }
            }
            catch
            {
            }
        }

        public event PongEventHandler OnPong;
        private async Task RaisePong(DateTime timestamp)
        {
            var e = OnPong;
            if (e != null)
            {
                try
                {
                    await e(timestamp);
                }
                catch
                {
                }
            }
        }

        public event ChannelCreatedHandler OnChannelCreated;
        private async Task RaiseOnChannelCreated(SlackChannelCreated chatHub)
        {
            var e = OnChannelCreated;
            if (e != null)
            {
                try
                {
                    await e(chatHub);
                }
                catch
                {
                }
            }
        }
        //TODO: USER JOINED EVENT HANDLING
    }
}
