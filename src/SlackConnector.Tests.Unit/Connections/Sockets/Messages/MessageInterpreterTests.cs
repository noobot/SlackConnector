using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Sockets.Messages
{
    internal class given_standard_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                  'type': 'message',
                  'channel': '&lt;myChannel&gt;',
                  'user': '&lt;myUser&gt;',
                  'text': 'hi, my name is &lt;noobot&gt;',
                  'ts': '1445366603.000002',
                  'team': '&lt;myTeam&gt;'
                }
            ";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_look_like_expected()
        {
            var expected = new ChatMessage
            {
                MessageType = MessageType.Message,
                Channel = "<myChannel>",
                User = "<myUser>",
                Text = "hi, my name is <noobot>",
                Team = "<myTeam>",
                RawData = Json,
                TimeStamp = 1445366603.000002
            };

            Result.ShouldLookLike(expected);
        }
    }

    internal class given_group_joined_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                  'type': 'group_joined',
                  'channel': {
                    id: 'test-group',
                    is_group: true
                  }
                }
            ";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_look_like_expected()
        {
            var expected = new GroupJoinedMessage
            {
                MessageType = MessageType.Group_Joined,
                Channel = new Group
                {
                    Id = "test-group",
                    IsGroup = true
                },
                RawData = Json,
            };

            Result.ShouldLookLike(expected);
        }
    }

    internal class given_channel_joined_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                  'type': 'channel_joined',
                  'channel': {
                    id: 'test-channel',
                    is_channel: true
                  }
                }
            ";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_look_like_expected()
        {
            var expected = new ChannelJoinedMessage
            {
                MessageType = MessageType.Channel_Joined,
                Channel = new Channel
                {
                    Id = "test-channel",
                    IsChannel = true
                },
                RawData = Json,
            };

            Result.ShouldLookLike(expected);
        }
    }

    internal class given_non_message_type_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"{ 'type': 'something_else' }";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_look_like_expected()
        {
            var expected = new ChatMessage
            {
                MessageType = MessageType.Unknown,
                RawData = Json
            };

            Result.ShouldLookLike(expected);
        }
    }

    internal class given_dodge_json_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"{ 'type': 'something_else', 'channel': { 'isObject': true } }";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_return_null()
        {
            Result.ShouldBeNull();
        }
    }

    internal class given_standard_message_with_null_data_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                  'type': 'message',
                  'channel': null,
                  'user': null,
                  'text': null,
                  'ts': '1445366603.000002',
                  'team': null
                }
            ";
        }

        protected override void When()
        {
            Result = SUT.InterpretMessage(Json);
        }

        [Test]
        public void then_should_look_like_expected()
        {
            var expected = new ChatMessage
            {
                MessageType = MessageType.Message,
                Channel = null,
                User = null,
                Text = null,
                Team = null,
                RawData = Json,
                TimeStamp = 1445366603.000002
            };

            Result.ShouldLookLike(expected);
        }
    }
}