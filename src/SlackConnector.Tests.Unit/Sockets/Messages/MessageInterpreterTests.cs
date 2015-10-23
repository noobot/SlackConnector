using System.Net;
using NUnit.Framework;
using SlackConnector.Sockets.Messages;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Sockets.Messages
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
            var expected = new InboundMessage
            {
                MessageType = MessageType.Message,
                Channel = "<myChannel>",
                User = "<myUser>",
                Text = "hi, my name is <noobot>",
                Team = "<myTeam>"
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
            var expected = new InboundMessage
            {
                MessageType = MessageType.Unknown,
            };

            Result.ShouldLookLike(expected);
        }
    }
}