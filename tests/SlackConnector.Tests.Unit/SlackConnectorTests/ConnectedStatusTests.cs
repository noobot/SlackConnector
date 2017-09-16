using System;
using System.Threading.Tasks;
using Moq;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public class ConnectedStatusTests
    {
        private string _slackKey = "slacKing-off-ey?";
        private string _webSocketUrl = "https://some-web-url";
        private readonly Mock<IHandshakeClient> _handshakeClient;
        private readonly Mock<IWebSocketClient> _webSocketClient;
        private readonly Mock<IConnectionFactory> _connectionFactory;
        private readonly Mock<ISlackConnectionFactory> _slackConnectionFactory;
        private readonly SlackConnector _slackConnector;

        public ConnectedStatusTests()
        {
            _handshakeClient = new Mock<IHandshakeClient>();
            _webSocketClient = new Mock<IWebSocketClient>();
            _connectionFactory = new Mock<IConnectionFactory>();
            _slackConnectionFactory = new Mock<ISlackConnectionFactory>();
            _slackConnector = new SlackConnector(_connectionFactory.Object, _slackConnectionFactory.Object);

            _connectionFactory
                .Setup(x => x.CreateHandshakeClient())
                .Returns(_handshakeClient.Object);

            _connectionFactory
                .Setup(x => x.CreateWebSocketClient(_webSocketUrl, null))
                .ReturnsAsync(_webSocketClient.Object);
        }

        [Fact]
        public async Task should_initialise_connection_with_websocket_and_return_expected_connection()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            var expectedConnection = new Mock<ISlackConnection>().Object;
            _slackConnectionFactory
                .Setup(x => x.Create(It.IsAny<ConnectionInformation>()))
                .ReturnsAsync(expectedConnection);

            // when
            var result = await _slackConnector.Connect(_slackKey);

            // then
            result.ShouldBe(expectedConnection);

            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.WebSocket == _webSocketClient.Object)), Times.Once);
            _connectionFactory.Verify(x => x.CreateWebSocketClient(_webSocketUrl, null));
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_self_details()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id", Name = "my-name" },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Self.Id == handshakeResponse.Self.Id)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Self.Name == handshakeResponse.Self.Name)), Times.Once);
        }

        [Fact]
        public async Task should_return_expected_connection()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id", Name = "my-name" },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            var expectedConnection = new Mock<ISlackConnection>().Object;
            _slackConnectionFactory
                .Setup(x => x.Create(It.IsAny<ConnectionInformation>()))
                .ReturnsAsync(expectedConnection);

            // when
            var result = await _slackConnector.Connect(_slackKey);

            // then
            result.ShouldBe(expectedConnection);
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_team_details()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Team = new Detail { Id = "team-id", Name = "team-name" },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Team.Id == handshakeResponse.Team.Id)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Team.Name == handshakeResponse.Team.Name)), Times.Once);
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_users_details()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Users = new[]
                {
                    new User { Id = "user-1-id", Name = "user-1-name" },
                    new User { Id = "user-2-id", Name = "user-2-name" },
                },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Users.Count == 2)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Users["user-1-id"].Name == "user-1-name")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.Users["user-2-id"].Name == "user-2-name")), Times.Once);
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_channel_that_bot_is_a_member_of()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel { Id = "i-am-a-channel", Name = "channel-name" , IsMember = true, Members = new [] { "member1", "member2" }},
                    new Channel { Id = "i-am-another-channel", Name = "but-you-aint-invited" , IsMember = false },
                    new Channel { Id = "i-am-archived-channel", Name = "please-ignore-me" , IsMember = true, IsArchived = true },
                },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 1)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-channel"].Id == "i-am-a-channel")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-channel"].Name == "#channel-name")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-channel"].Type == SlackChatHubType.Channel)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-channel"].Members == handshakeResponse.Channels[0].Members)), Times.Once);
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_groups_that_bot_is_a_member_of()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id" },
                Groups = new[]
                {
                    new Group { Id = "i-am-a-group", Name = "group-name", Members = new [] {"my-id", "another-member"} },
                    new Group { Id = "i-am-another-group", Name = "and-you-aint-a-member-of-it", Members = null },
                    new Group { Id = "i-am-a-group", Name = "group-name", Members = new [] {"my-id"}, IsArchived = true },
                },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 1)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-group"].Id == "i-am-a-group")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-group"].Name == "#group-name")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-group"].Type == SlackChatHubType.Group)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-group"].Members == handshakeResponse.Groups[0].Members)), Times.Once);
        }

        [Fact]
        public async Task should_initialise_connection_with_expected_ims()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id" },
                Users = new[]
                {
                    new User { Id = "user-guid-thingy", Name = "expected-name" },
                },
                Ims = new[]
                {
                    new Im { Id = "i-am-a-im", User = "user-i-am_yup" },
                    new Im { Id = "user-with-name", User = "user-guid-thingy" },
                },
                WebSocketUrl = _webSocketUrl
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            await _slackConnector.Connect(_slackKey);

            // then
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs.Count == 2)), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-im"].Id == "i-am-a-im")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-im"].Name == "@user-i-am_yup")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["i-am-a-im"].Type == SlackChatHubType.DM)), Times.Once);

            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["user-with-name"].Id == "user-with-name")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["user-with-name"].Name == "@expected-name")), Times.Once);
            _slackConnectionFactory
                .Verify(x => x.Create(It.Is((ConnectionInformation p) => p.SlackChatHubs["user-with-name"].Type == SlackChatHubType.DM)), Times.Once);
        }

        [Fact]
        public async Task should_throw_exception_when_handshake_is_not_ok()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = false,
                Error = "I AM A ERROR"
            };

            _handshakeClient
                .Setup(x => x.FirmShake(_slackKey))
                .ReturnsAsync(handshakeResponse);

            // when
            var exception = await Assert.ThrowsAsync<HandshakeException>(() => _slackConnector.Connect(_slackKey));

            // then
            exception.Message.ShouldBe(handshakeResponse.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task should_throw_exception_given_empty_api_key(string slackKey)
        {
            // given
            
            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _slackConnector.Connect(slackKey));

            // then
            exception.Message.ShouldContain("slackKey");
        }
    }
}