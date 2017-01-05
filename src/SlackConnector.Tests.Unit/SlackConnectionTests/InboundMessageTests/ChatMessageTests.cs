using System;
using System.Linq;
using NUnit.Framework;
using Should;
using SlackConnector.BotHelpers;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal abstract class ChatMessageTest : BaseTest<ChatMessage>
    {
        protected override void When()
        {
            SUT.Initialise(ConnectionInfo);

            if (!string.IsNullOrEmpty(InboundMessage?.Channel))
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnMessage += null, null, new ChannelJoinedMessage { Channel = new Channel { Id = InboundMessage.Channel } });
            }

            GetMockFor<IWebSocketClient>()
                .Raise(x => x.OnMessage += null, null, InboundMessage);
        }
    }

    internal class given_connector_is_setup_when_inbound_message_arrives : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            ConnectionInfo.Users.Add("userABC", new SlackUser() { Id = "userABC", Name = "i-have-a-name" });

            InboundMessage = new ChatMessage
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

    internal class given_connector_is_setup_when_inbound_direct_message_arrives_from_a_user_for_the_first_time : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            ConnectionInfo.Users.Add("userABC", new SlackUser() { Id = "userABC", Name = "i-have-a-name" });

            InboundMessage = new ChatMessage
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

    internal class given_connector_is_missing_use_when_inbound_message_arrives : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            InboundMessage = new ChatMessage
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

    internal class given_connector_is_setup_when_inbound_message_arrives_that_isnt_message_type : ChatMessageTest
    {
        protected override void Given()
        {
            InboundMessage = new ChatMessage
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

    internal class given_null_message_when_inbound_message_arrives : ChatMessageTest
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

    internal class given_channel_already_defined_when_inbound_message_arrives_with_channel : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            ConnectionInfo.SlackChatHubs.Add("channelId", new SlackChatHub { Id = "channelId", Name = "NaMe23" });

            InboundMessage = new ChatMessage
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


    internal class given_bot_was_mentioned_in_text : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            ConnectionInfo.Self = new ContactDetails { Id = "self-id", Name = "self-name" };

            InboundMessage = new ChatMessage
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

    internal class given_message_is_from_self : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            ConnectionInfo.Self = new ContactDetails { Id = "self-id", Name = "self-name" };

            InboundMessage = new ChatMessage
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

    internal class given_message_is_missing_user_information : ChatMessageTest
    {
        protected override void Given()
        {
            base.Given();

            InboundMessage = new ChatMessage
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
    internal class given_exception_thrown_when_handling_inbound_message : ChatMessageTest
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
            var message = new ChatMessage
            {
                User = "something",
                MessageType = MessageType.Message
            };

            WebSocket.RaiseOnMessage(message);
        }
    }
}
