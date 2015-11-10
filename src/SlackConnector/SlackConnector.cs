using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ISlackConnectionFactory _slackConnectionFactory;

        public SlackConnector() : this(new ConnectionFactory(), new SlackConnectionFactory())
        { }

        internal SlackConnector(IConnectionFactory connectionFactory, ISlackConnectionFactory slackConnectionFactory)
        {
            _connectionFactory = connectionFactory;
            _slackConnectionFactory = slackConnectionFactory;
        }

        public async Task<ISlackConnection> Connect(string slackKey)
        {
            if (string.IsNullOrEmpty(slackKey))
            {
                throw new ArgumentNullException(nameof(slackKey));
            }

            var handshakeClient = _connectionFactory.CreateHandshakeClient();
            SlackHandshake handshake = await handshakeClient.FirmShake(slackKey);

            if (!handshake.Ok)
            {
                throw new HandshakeException(handshake.Error);
            }

            var connectionInfo = new ConnectionInformation
            {
                Self = new ContactDetails { Id = handshake.Self.Id, Name = handshake.Self.Name },
                Team = new ContactDetails { Id = handshake.Team.Id, Name = handshake.Team.Name },
                Users = GenerateUsers(handshake.Users),
                SlackChatHubs = GetChatHubs(handshake),
                WebSocket = await GetWebSocket(handshake)
            };

            return _slackConnectionFactory.Create(connectionInfo);
        }

        private async Task<IWebSocketClient> GetWebSocket(SlackHandshake handshake)
        {
            var webSocketClient = _connectionFactory.CreateWebSocketClient(handshake.WebSocketUrl);
            await webSocketClient.Connect();
            return webSocketClient;
        }

        private Dictionary<string, string> GenerateUsers(User[] users)
        {
            return users.ToDictionary(user => user.Id, user => user.Name);
        }

        private Dictionary<string, SlackChatHub> GetChatHubs(SlackHandshake handshake)
        {
            var hubs = new Dictionary<string, SlackChatHub>();

            foreach (Channel channel in handshake.Channels)
            {
                if (!channel.IsArchived)
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

            foreach (Group group in handshake.Groups)
            {
                if (!group.IsArchived)
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

            foreach (Im im in handshake.Ims)
            {
                var dm = new SlackChatHub
                {
                    Id = im.Id,
                    Name = "@" + im.Id,
                    Type = SlackChatHubType.DM
                };

                hubs.Add(im.Id, dm);
            }

            return hubs;
        }
    }
}