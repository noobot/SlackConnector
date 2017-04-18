using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public class ConnectedStatusTests
    {
        private string _slackKey = "slacKing-off-ey?";
        private string _webSocketUrl = "https://some-web-url";
        private Mock<IHandshakeClient> _handshakeClient;
        private Mock<IWebSocketClient> _webSocketClient;
        private Mock<IConnectionFactory> _connectionFactory;
        private Mock<ISlackConnectionFactory> _slackConnectionFactory;
        private SlackConnector _slackConnector;

        [SetUp]
        public void Setup()
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

        [Test, AutoMoqData]
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

        [Test, AutoMoqData]
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
            result.ShouldEqual(expectedConnection);
        }

        [Test, AutoMoqData]
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

        public class given_valid_setup_when_connected : SpecsFor<SlackConnector>
        {
            private const string SlackKey = "slacKing-off-ey?";
            private SlackConnectionFactoryStub SlackFactoryStub { get; set; }
            private SlackConnectionStub Connection { get; set; }
            private HandshakeResponse HandshakeResponse { get; set; }
            private ISlackConnection Result { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SlackFactoryStub = new SlackConnectionFactoryStub();
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, SlackFactoryStub);
            }

            protected override void Given()
            {
                HandshakeResponse = new HandshakeResponse
                {
                    Ok = true,
                    Self = new Detail { Id = "my-id", Name = "my-name" },
                    Team = new Detail { Id = "team-id", Name = "team-name" },
                    Users = new[]
                    {
                        new User { Id = "user-1-id", Name = "user-1-name" },
                        new User { Id = "user-2-id", Name = "user-2-name" },
                    },
                    Channels = new[]
                    {
                        new Channel { Id = "i-am-a-channel", Name = "channel-name" , IsMember = true }
                    },
                    Groups = new[]
                    {
                        new Group { Id = "i-am-a-group", Name = "group-name", Members = new [] {"my-id"} },
                    },
                    Ims = new[]
                    {
                        new Im { Id = "i-am-a-im", User = "user-i-am_yup"}
                    },
                    WebSocketUrl = "some-valid-url"
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(SlackKey))
                    .ReturnsAsync(HandshakeResponse);

                Connection = new SlackConnectionStub();
                SlackFactoryStub.Create_Value = Connection;

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(HandshakeResponse.WebSocketUrl, null))
                    .ReturnsAsync(GetMockFor<IWebSocketClient>().Object);

                GetMockFor<IWebSocketClient>()
                    .Setup(x => x.Connect(HandshakeResponse.WebSocketUrl))
                    .Returns(Task.CompletedTask);
            }

            protected override void When()
            {
                Result = SUT.Connect(SlackKey).Result;
            }

            [Test]
            public void then_should_pass_expected_users()
            {
                var users = SlackFactoryStub.Create_ConnectionInformation.Users;
                users.ShouldNotBeNull();
                users.Count.ShouldEqual(2);
                users[HandshakeResponse.Users[0].Id].Name.ShouldEqual(HandshakeResponse.Users[0].Name);
                users[HandshakeResponse.Users[1].Id].Name.ShouldEqual(HandshakeResponse.Users[1].Name);
            }

            [Test]
            public void then_should_pass_expected_channels()
            {
                Dictionary<string, SlackChatHub> hubs = SlackFactoryStub.Create_ConnectionInformation.SlackChatHubs;
                hubs.ShouldNotBeNull();
                hubs.Count.ShouldBeGreaterThan(0);

                var hub = hubs[HandshakeResponse.Channels[0].Id];
                hub.ShouldNotBeNull();
                hub.Id.ShouldEqual(HandshakeResponse.Channels[0].Id);
                hub.Name.ShouldEqual("#" + HandshakeResponse.Channels[0].Name);
                hub.Type.ShouldEqual(SlackChatHubType.Channel);
            }

            [Test]
            public void then_should_pass_expected_groups()
            {
                Dictionary<string, SlackChatHub> hubs = SlackFactoryStub.Create_ConnectionInformation.SlackChatHubs;
                hubs.ShouldNotBeNull();
                hubs.Count.ShouldBeGreaterThan(0);

                var hub = hubs[HandshakeResponse.Groups[0].Id];
                hub.ShouldNotBeNull();
                hub.Id.ShouldEqual(HandshakeResponse.Groups[0].Id);
                hub.Name.ShouldEqual("#" + HandshakeResponse.Groups[0].Name);
                hub.Type.ShouldEqual(SlackChatHubType.Group);
            }

            [Test]
            public void then_should_pass_expected_ims()
            {
                Dictionary<string, SlackChatHub> hubs = SlackFactoryStub.Create_ConnectionInformation.SlackChatHubs;
                hubs.ShouldNotBeNull();
                hubs.Count.ShouldBeGreaterThan(0);

                var hub = hubs[HandshakeResponse.Ims[0].Id];
                hub.ShouldNotBeNull();
                hub.Id.ShouldEqual(HandshakeResponse.Ims[0].Id);
                hub.Name.ShouldEqual("@" + HandshakeResponse.Ims[0].User);
                hub.Type.ShouldEqual(SlackChatHubType.DM);
            }

            [Test]
            public void then_should_pass_in_expected_websocket()
            {
                var webSocket = SlackFactoryStub.Create_ConnectionInformation.WebSocket;
                webSocket.ShouldEqual(GetMockFor<IWebSocketClient>().Object);
            }

            [Test]
            public void then_should_pass_in_slack_key()
            {
                string key = SlackFactoryStub.Create_ConnectionInformation.SlackKey;
                key.ShouldEqual(SlackKey);
            }
        }

        public class given_handshake_was_not_ok : SpecsFor<SlackConnector>
        {
            private HandshakeResponse HandshakeResponse { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, GetMockFor<ISlackConnectionFactory>().Object);
            }

            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                HandshakeResponse = new HandshakeResponse { Ok = false, Error = "I AM A ERROR" };
                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(HandshakeResponse);
            }

            [Test]
            public void then_should_throw_exception()
            {
                HandshakeException exception = null;

                try
                {
                    SUT.Connect("something").Wait();
                }
                catch (AggregateException ex)
                {

                    exception = ex.InnerExceptions[0] as HandshakeException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo(HandshakeResponse.Error));
            }
        }

        public class given_empty_api_key : SpecsFor<SlackConnector>
        {
            protected override void InitializeClassUnderTest()
            {
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, GetMockFor<ISlackConnectionFactory>().Object);
            }

            [Test]
            public void then_should_be_aware_of_current_state()
            {
                bool exceptionDetected = false;

                try
                {
                    SUT.Connect("").Wait();
                }
                catch (AggregateException ex)
                {
                    exceptionDetected = ex.InnerExceptions[0] is ArgumentNullException;
                }

                Assert.That(exceptionDetected, Is.True);
            }
        }

        public class given_valid_setup_when_connecting_with_a_proxy_connection : SpecsFor<SlackConnector>
        {
            private const string SlackKey = "slacKing-off-ey?";
            private SlackConnectionFactoryStub SlackFactoryStub { get; set; }
            private SlackConnectionStub Connection { get; set; }
            private ISlackConnection Result { get; set; }
            private ProxySettings ProxySettings { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SlackFactoryStub = new SlackConnectionFactoryStub();
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, SlackFactoryStub);
            }

            protected override void Given()
            {
                var handshakeResponse = new HandshakeResponse
                {
                    Ok = true,
                    WebSocketUrl = "some-valid-url"
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(SlackKey))
                    .ReturnsAsync(handshakeResponse);

                Connection = new SlackConnectionStub();
                SlackFactoryStub.Create_Value = Connection;

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                ProxySettings = new ProxySettings("hi", "you", "ok?");
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(handshakeResponse.WebSocketUrl, ProxySettings))
                    .ReturnsAsync(GetMockFor<IWebSocketClient>().Object);
            }

            protected override void When()
            {
                Result = SUT.Connect(SlackKey, ProxySettings).Result;
            }

            [Test]
            public void then_should_return_expected_connection()
            {
                Result.ShouldEqual(Connection);
            }
        }
    }
}