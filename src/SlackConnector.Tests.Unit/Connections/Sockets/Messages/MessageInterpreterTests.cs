using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Logging;
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

    internal class given_dm_channel_joined_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                   'type':'im_created',
                   'channel':{
                      'id':'D99999',
                      'user':'U99999',
                      'is_im':true,
                      'is_open':true
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
            var expected = new DmChannelJoinedMessage
            {
                MessageType = MessageType.Im_Created,
                Channel = new Im
                {
                    Id = "D99999",
                    User = "U99999",
                    IsIm = true,
                    IsOpen = true
                },
                RawData = Json,
            };

            Result.ShouldLookLike(expected);
        }
    }

    internal class given_user_joined_message_when_processing_message : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {  
                   'type':'team_join',
                   'user':{  
                      'id':'U3339999',
                      'name':'tmp',
                      'profile':{  
                         'real_name':'temp-name'
                      },
                      'is_admin':false,
                      'is_bot':true
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
            var expected = new UserJoinedMessage
            {
                MessageType = MessageType.Team_Join,
                User = new User
                {
                    Id = "U3339999",
                    Name = "tmp",
                    IsAdmin = false,
                    IsBot = true,
                    Profile = new Profile
                    {
                        RealName = "temp-name"
                    }
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

    internal class given_no_data_when_processing_message_with_log_level_of_none : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = null;
            SlackConnector.LoggingLevel = ConsoleLoggingLevel.None;
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


        [Test]
        public void then_shouldnt_log_anything()
        {
            GetMockFor<ILogger>()
                .Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
        }
    }

    internal class given_no_data_when_processing_message_with_log_level_of_greater_than_none: SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = null;
            SlackConnector.LoggingLevel = ConsoleLoggingLevel.All;
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


        [Test]
        public void then_should_log_something()
        {
            GetMockFor<ILogger>()
                .Verify(x => x.LogError(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}