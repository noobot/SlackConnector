using System.Linq;
using System.Threading.Tasks;
using Moq;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public class HubsTests
    {
        private string _webSocketUrl = "some-web-url";
        private readonly Mock<IHandshakeClient> _handshakeClient;
        private readonly Mock<ISlackConnectionFactory> _slackConnectionFactory;
        private readonly SlackConnector _slackConnector;

        public HubsTests()
        {
            _handshakeClient = new Mock<IHandshakeClient>();
            var webSocketClient = new Mock<IWebSocketClient>();
            var connectionFactory = new Mock<IConnectionFactory>();
            _slackConnectionFactory = new Mock<ISlackConnectionFactory>();
            _slackConnector = new SlackConnector(connectionFactory.Object, _slackConnectionFactory.Object);

            connectionFactory
                .Setup(x => x.CreateHandshakeClient())
                .Returns(_handshakeClient.Object);

            connectionFactory
                .Setup(x => x.CreateWebSocketClient(_webSocketUrl, null))
                .ReturnsAsync(webSocketClient.Object);
        }

        [Fact]
        public async Task should_not_contain_archived_channels()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel
                    {
                        Id = "Id1",
                        Name = "Name1",
                        IsArchived = true,
                        IsMember = true //TODO: Need to do self things
                    },
                    new Channel
                    {
                        Id = "Id2",
                        Name = "Name2",
                        IsArchived = true,
                        IsMember = true //TODO: Need to do self things
                    },
                }
            };

            _handshakeClient
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect("something");

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task should_not_contain_channels_that_are_archived_and_bot_is_not_a_member_off()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel
                    {
                        Id = "Id1",
                        Name = "Name1",
                        IsArchived = false,
                        IsMember = false
                    }
                }
            };

            _handshakeClient
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect("something");

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task should_not_contain_archived_groups()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Groups = new[]
                {
                    new Group
                    {
                        Id = "group-id",
                        Name = "group-name",
                        IsArchived = true
                    }
                }
            };

            _handshakeClient
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect("something");

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task should_not_contain_groups_that_are_archived_and_bot_is_not_a_member_off()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Groups = new[]
                {
                    new Group
                    {
                        Id = "group-id",
                        Name = "group-name",
                        IsArchived = false,
                        Members = new [] { "something-else" }
                    }
                }
            };

            _handshakeClient
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect("something");

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task should_contain_channel_and_username()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Users = new[]
                {
                    new User
                    {
                        Id = "user-id-thingy",
                        Name = "name-4eva"
                    }
                },
                Ims = new[]
                {
                    new Im
                    {
                        Id = "im-id-yay",
                        User = "user-id-thingy"
                    }
                }
            };

            _handshakeClient
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect("something");

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 1)), Times.Once);

            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.First().Value.Name == "@" + handshakeResponse.Users[0].Name)), Times.Once);
        }
    }
}