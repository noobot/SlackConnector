using NUnit.Framework;
using SlackConnector.Sockets.Messages;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Sockets.Messages
{
    internal class MessageInterpreterTests : SpecsFor<MessageInterpreter>
    {
        private string Json { get; set; }
        private InboundMessage Result { get; set; }

        protected override void Given()
        {
            Json = @"
                {
                  'type': 'message',
                  'channel': 'myChannel',
                  'user': 'myUser',
                  'text': 'hi',
                  'ts': '1445366603.000002',
                  'team': 'myTeam'
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
                Channel = "myChannel",
                User = "myUser",
                Text = "hi",
                Team = "myTeam"
            };

            Result.ShouldLookLike(expected);
        }
    }
}