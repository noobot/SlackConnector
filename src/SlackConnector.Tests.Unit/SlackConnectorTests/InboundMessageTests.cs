using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.SlackConnectorTests.Setups;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class InboundMessageTests
    {
        internal class BaseTest : ValidSetup
        {
            protected InboundMessage InboundMessage { get; set; }
            protected bool MessageRaised { get; set; }
            protected SlackMessage Result { get; set; }
            protected SlackHandshake Handshake { get; set; }

            protected override void Given()
            {
                base.Given();

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(Handshake ?? new SlackHandshake());

                SUT.OnMessageReceived += async message =>
                {
                    Result = message;
                    MessageRaised = true;
                    await Task.Factory.StartNew(() => { });
                };

                SUT.Connect("blah").Wait();
            }

            protected override void When()
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnMessage += null, null, InboundMessage);
            }
        }

        internal class given_connector_is_setup_when_inbound_message_arrives : BaseTest
        {
            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Users = new[]
                    {
                        new User
                        {
                            Id = "userABC",
                            Name = "I-have-a-name"
                        },
                    }
                };

                InboundMessage = new InboundMessage
                {
                    User = "userABC",
                    MessageType = MessageType.Message,
                    Text = "amazing-text",
                    RawData = "I am raw data yo"
                };

                base.Given();
            }

            [Test]
            public void then_should_raise_event()
            {
                MessageRaised.ShouldBeTrue();
            }

            [Test]
            public void then_should_pass_through_expected_message()
            {
                var expected = new SlackMessage
                {
                    Text = "amazing-text",
                    User = new SlackUser
                    {
                        Id = "userABC",
                        Name = "I-have-a-name"
                    },
                    RawData = InboundMessage.RawData
                };

                Result.ShouldLookLike(expected);
            }
        }

        internal class given_connector_is_missing_use_when_inbound_message_arrives : BaseTest
        {
            protected override void Given()
            {
                InboundMessage = new InboundMessage
                {
                    User = "userABC",
                    MessageType = MessageType.Message
                };

                base.Given();
            }

            [Test]
            public void then_should_pass_through_expected_message()
            {
                var expected = new SlackMessage
                {
                    User = new SlackUser
                    {
                        Id = "userABC",
                        Name = string.Empty
                    }
                };

                Result.ShouldLookLike(expected);
            }
        }

        internal class given_connector_is_setup_when_inbound_message_arrives_that_isnt_message_type : BaseTest
        {
            protected override void Given()
            {
                InboundMessage = new InboundMessage
                {
                    MessageType = MessageType.Unknown
                };

                base.Given();
            }

            [Test]
            public void then_should_not_call_callback()
            {
                MessageRaised.ShouldBeFalse();
            }
        }

        internal class given_null_message_when_inbound_message_arrives : BaseTest
        {
            protected override void Given()
            {
                InboundMessage = null;

                base.Given();
            }

            [Test]
            public void then_should_not_call_callback()
            {
                MessageRaised.ShouldBeFalse();
            }
        }

        internal class given_channel_already_defined_when_inbound_message_arrives_with_channel : BaseTest
        {
            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Channels = new[]
                    {
                        new Channel
                        {
                            Id = "channelId",
                            Name = "NaMe23",
                            IsArchived = false
                        }
                    }
                };

                InboundMessage = new InboundMessage
                {
                    Channel = Handshake.Channels[0].Id,
                    MessageType = MessageType.Message,
                    User = "irmBrady"
                };

                base.Given();
            }

            [Test]
            public void then_should_return_expected_channel_information()
            {
                var expected = new SlackChatHub
                {
                    Id = Handshake.Channels[0].Id,
                    Name = "#" + Handshake.Channels[0].Name,
                    Type = SlackChatHubType.Channel
                };

                Result.ChatHub.ShouldLookLike(expected);
            }
        }

        internal class given_channel_undefined_when_inbound_message_arrives_with_channel : BaseTest
        {
            private readonly string _hubId = "Woozah";
            private readonly SlackChatHub _expectedChatHub = new SlackChatHub();

            protected override void Given()
            {
                GetMockFor<IChatHubInterpreter>()
                    .Setup(x => x.FromId(_hubId))
                    .Returns(_expectedChatHub);

                InboundMessage = new InboundMessage
                {
                    Channel = _hubId,
                    MessageType = MessageType.Message,
                    User = "something else"
                };

                base.Given();
            }

            [Test]
            public void then_should_return_expected_channel_information()
            {
                Result.ChatHub.ShouldEqual(_expectedChatHub);
            }

            [Test]
            public void then_should_add_channel_to_connected_hubs()
            {
                SUT.ConnectedHubs.ContainsKey(_hubId).ShouldBeTrue();
                SUT.ConnectedHubs[_hubId].ShouldEqual(_expectedChatHub);
            }
        }

        internal class given_bot_was_mentioned_in_text : BaseTest
        {
            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Self = new Detail { Id = "self-id", Name = "self-name" }
                };

                InboundMessage = new InboundMessage
                {
                    Channel = "idy",
                    MessageType = MessageType.Message,
                    Text = "please send help... :-p",
                    User = "lalala"
                };

                GetMockFor<IMentionDetector>()
                    .Setup(x => x.WasBotMentioned(Handshake.Self.Name, Handshake.Self.Id, InboundMessage.Text))
                    .Returns(true);

                base.Given();
            }

            [Test]
            public void then_should_return_expected_channel_information()
            {
                Result.MentionsBot.ShouldBeTrue();
            }
        }

        internal class given_message_is_from_self : BaseTest
        {
            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Self = new Detail { Id = "self-id", Name = "self-name" }
                };

                InboundMessage = new InboundMessage
                {
                    MessageType = MessageType.Message,
                    User = Handshake.Self.Id
                };

                GetMockFor<IMentionDetector>()
                    .Setup(x => x.WasBotMentioned(Handshake.Self.Name, Handshake.Self.Id, InboundMessage.Text))
                    .Returns(true);

                base.Given();
            }

            [Test]
            public void then_should_not_raise_message()
            {
                MessageRaised.ShouldBeFalse();
            }
        }

        internal class given_message_is_missing_user_information : BaseTest
        {
            protected override void Given()
            {
                InboundMessage = new InboundMessage
                {
                    MessageType = MessageType.Message,
                    User = null
                };

                base.Given();
            }

            [Test]
            public void then_should_not_raise_message()
            {
                MessageRaised.ShouldBeFalse();
            }
        }

        [TestFixture]
        internal class given_exception_thrown_when_handling_inbound_message : BaseTest
        {
            private readonly WebSocketClientStub WebSocket = new WebSocketClientStub();
            
            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(new SlackHandshake());

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(It.IsAny<string>()))
                    .Returns(WebSocket);

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(Handshake ?? new SlackHandshake());

                SUT.OnMessageReceived += message =>
                {
                    throw new NotImplementedException();
                };

                SUT.Connect("asdasd").Wait();
            }

            [Test]
            public void should_not_throw_exception_when_error_is_thrown()
            {
                var message = new InboundMessage
                {
                    User = "something",
                    MessageType = MessageType.Message
                };

                WebSocket.RaiseOnMessage(message);
            }
        }
    }
}