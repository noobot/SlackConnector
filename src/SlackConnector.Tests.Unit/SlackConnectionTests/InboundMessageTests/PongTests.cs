using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Connections.Sockets.Messages.Inbound;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class given_pong : BaseTest<PongMessage>
    {
        private DateTime _timestamp;

        protected override void Given()
        {
            base.Given();

            SUT.OnPong += timestamp =>
            {
                _timestamp = timestamp;
                return Task.CompletedTask;
            };

            InboundMessage = new PongMessage
            {
                Timestamp = DateTime.Now
            };
        }

        [Test]
        public void then_should_raised_event_with_expected_timestamp()
        {
            Assert.That(_timestamp, Is.EqualTo(InboundMessage.Timestamp));
        }
    }
}