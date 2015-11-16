using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class ConnectedStatusTests
    {
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
                    .Setup(x => x.CreateWebSocketClient(HandshakeResponse.WebSocketUrl))
                    .Returns(GetMockFor<IWebSocketClient>().Object);

                GetMockFor<IWebSocketClient>()
                    .Setup(x => x.Connect())
                    .Returns(Task.Factory.StartNew(() => { }));
            }

            protected override void When()
            {
                Result = SUT.Connect(SlackKey).Result;
            }

            [Test]
            public void then_should_return_expected_connection()
            {
                Result.ShouldEqual(Connection);
            }

            [Test]
            public void then_should_pass_in_self_details()
            {
                ContactDetails self = SlackFactoryStub.Create_ConnectionInformation.Self;
                self.ShouldNotBeNull();
                self.Id.ShouldEqual(HandshakeResponse.Self.Id);
                self.Name.ShouldEqual(HandshakeResponse.Self.Name);
            }

            [Test]
            public void then_should_pass_in_team_details()
            {
                ContactDetails team = SlackFactoryStub.Create_ConnectionInformation.Team;
                team.ShouldNotBeNull();
                team.Id.ShouldEqual(HandshakeResponse.Team.Id);
                team.Name.ShouldEqual(HandshakeResponse.Team.Name);
            }

            [Test]
            public void then_should_pass_expected_users()
            {
                Dictionary<string, string> users = SlackFactoryStub.Create_ConnectionInformation.Users;
                users.ShouldNotBeNull();
                users.Count.ShouldEqual(2);
                users[HandshakeResponse.Users[0].Id].ShouldEqual(HandshakeResponse.Users[0].Name);
                users[HandshakeResponse.Users[1].Id].ShouldEqual(HandshakeResponse.Users[1].Name);
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
            public void then_should_connect_websocket()
            {
                GetMockFor<IWebSocketClient>()
                    .Verify(x => x.Connect());
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
    }
}