using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector.Connections;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        public static ConsoleLoggingLevel LoggingLevel = ConsoleLoggingLevel.FatalErrors;

        private readonly IConnectionFactory _connectionFactory;
        private readonly ISlackConnectionFactory _slackConnectionFactory;

        public SlackConnector() : this(new ConnectionFactory(), new SlackConnectionFactory())
        { }

        internal SlackConnector(IConnectionFactory connectionFactory, ISlackConnectionFactory slackConnectionFactory)
        {
            _connectionFactory = connectionFactory;
            _slackConnectionFactory = slackConnectionFactory;
        }

        public async Task<ISlackConnection> Connect(string slackKey, ProxySettings proxySettings = null)
        {
            if (string.IsNullOrEmpty(slackKey))
            {
                throw new ArgumentNullException(nameof(slackKey));
            }

            var handshakeClient = _connectionFactory.CreateHandshakeClient();
            HandshakeResponse handshakeResponse = await handshakeClient.FirmShake(slackKey);

            if (!handshakeResponse.Ok)
            {
                throw new HandshakeException(handshakeResponse.Error);
            }

            var connectionInfo = new ConnectionInformation
            {
                SlackKey = slackKey,
                Self = new ContactDetails { Id = handshakeResponse.Self.Id, Name = handshakeResponse.Self.Name },
                Team = new ContactDetails { Id = handshakeResponse.Team.Id, Name = handshakeResponse.Team.Name },
                Users = GenerateUsers(handshakeResponse.Users),
                SlackChatHubs = GetChatHubs(handshakeResponse),
                WebSocket = await GetWebSocket(handshakeResponse, proxySettings)
            };

            return _slackConnectionFactory.Create(connectionInfo);
        }

        private async Task<IWebSocketClient> GetWebSocket(HandshakeResponse handshakeResponse, ProxySettings proxySettings)
        {
            var webSocketClient = _connectionFactory.CreateWebSocketClient(handshakeResponse.WebSocketUrl, proxySettings);
            await webSocketClient.Connect();
            return webSocketClient;
        }

        private Dictionary<string, string> GenerateUsers(User[] users)
        {
            return users.ToDictionary(user => user.Id, user => user.Name);
        }

        private Dictionary<string, SlackChatHub> GetChatHubs(HandshakeResponse handshakeResponse)
        {
            var hubs = new Dictionary<string, SlackChatHub>();

            foreach (Channel channel in handshakeResponse.Channels.Where(x => !x.IsArchived))
            {
                if (channel.IsMember)
                {
                    var newChannel = new SlackChatHub
                    {
                        Id = channel.Id,
                        Name = "#" + channel.Name,
                        Type = SlackChatHubType.Channel
                    };

                    hubs.Add(channel.Id, newChannel);
                }
            }

            foreach (Group group in handshakeResponse.Groups.Where(x => !x.IsArchived))
            {
                if (group.Members.Any(x => x == handshakeResponse.Self.Id))
                {
                    var newGroup = new SlackChatHub
                    {
                        Id = group.Id,
                        Name = "#" + group.Name,
                        Type = SlackChatHubType.Group
                    };

                    hubs.Add(group.Id, newGroup);
                }
            }

            foreach (Im im in handshakeResponse.Ims)
            {
                User user = handshakeResponse.Users.FirstOrDefault(x => x.Id == im.User);
                var dm = new SlackChatHub
                {
                    Id = im.Id,
                    Name = "@" + (user == null ? im.User : user.Name),
                    Type = SlackChatHubType.DM
                };

                hubs.Add(im.Id, dm);
            }

            return hubs;
        }
    }
}