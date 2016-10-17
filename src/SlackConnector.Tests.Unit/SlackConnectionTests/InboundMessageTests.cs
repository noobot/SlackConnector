using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using SlackConnector.BotHelpers;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class InboundMessageTests
    {
        internal class BaseTest : SpecsFor<SlackConnection>
        {
            protected InboundMessage InboundMessage { get; set; }
            protected bool MessageRaised { get; set; }
            protected SlackMessage Result { get; set; }
            protected ConnectionInformation ConnectionInfo { get; set; }

            protected override void Given()
            {
                SUT.OnMessageReceived += async message =>
                {
                    Result = message;
                    MessageRaised = true;
                    await Task.Factory.StartNew(() => { });
                };

                ConnectionInfo = new ConnectionInformation { WebSocket = GetMockFor<IWebSocketClient>().Object };
            }

            protected override void When()
            {
                SUT.Initialise(ConnectionInfo);
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnMessage += null, null, InboundMessage);
            }
        }

        internal class given_connector_is_setup_when_inbound_message_arrives : BaseTest
        {
            protected override void Given()
            {
                base.Given();

                ConnectionInfo.Users.Add("userABC", new SlackUser() {Id = "userABC", Name = "i-have-a-name" });

                InboundMessage = new InboundMessage
                {
                    User = "userABC",
                    MessageType = MessageType.Message,
                    Text = "amazing-text",
                    RawData = "I am raw data yo"
                };
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
                        Name = "i-have-a-name"
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
                base.Given();

                InboundMessage = new InboundMessage
                {
                    User = "userABC",
                    MessageType = MessageType.Message
                };
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
                base.Given();

                ConnectionInfo.SlackChatHubs.Add("channelId", new SlackChatHub { Id = "channelId", Name = "NaMe23" });

                InboundMessage = new InboundMessage
                {
                    Channel = ConnectionInfo.SlackChatHubs.First().Key,
                    MessageType = MessageType.Message,
                    User = "irmBrady"
                };
            }

            [Test]
            public void then_should_return_expected_channel_information()
            {
                SlackChatHub expected = ConnectionInfo.SlackChatHubs.First().Value;
                Result.ChatHub.ShouldEqual(expected);
            }
        }

        internal class given_channel_undefined_when_inbound_message_arrives_with_channel : BaseTest
        {
            private readonly string _hubId = "Woozah";
            private readonly SlackChatHub _expectedChatHub = new SlackChatHub();

            protected override void Given()
            {
                base.Given();

                GetMockFor<IChatHubInterpreter>()
                    .Setup(x => x.FromId(_hubId))
                    .Returns(_expectedChatHub);

                InboundMessage = new InboundMessage
                {
                    Channel = _hubId,
                    MessageType = MessageType.Message,
                    User = "something else"
                };
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
                base.Given();

                ConnectionInfo.Self = new ContactDetails { Id = "self-id", Name = "self-name" };
                
                InboundMessage = new InboundMessage
                {
                    Channel = "idy",
                    MessageType = MessageType.Message,
                    Text = "please send help... :-p",
                    User = "lalala"
                };

                GetMockFor<IMentionDetector>()
                    .Setup(x => x.WasBotMentioned(ConnectionInfo.Self.Name, ConnectionInfo.Self.Id, InboundMessage.Text))
                    .Returns(true);
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
                base.Given();

                ConnectionInfo.Self = new ContactDetails { Id = "self-id", Name = "self-name" };
                
                InboundMessage = new InboundMessage
                {
                    MessageType = MessageType.Message,
                    User = ConnectionInfo.Self.Id
                };
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
                base.Given();

                InboundMessage = new InboundMessage
                {
                    MessageType = MessageType.Message,
                    User = null
                };
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
            private WebSocketClientStub WebSocket { get; set; }

            protected override void Given()
            {
                base.Given();

                WebSocket = new WebSocketClientStub();
                ConnectionInfo.WebSocket = WebSocket;

                SUT.OnMessageReceived += message =>
                {
                    throw new NotImplementedException();
                };
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