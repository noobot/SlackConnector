using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class PongTests
    {
        [Test, AutoMoqData]
        public async Task should_raise_event(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);
            
            DateTime lastTimestamp = DateTime.MinValue;
            slackConnection.OnPong += timestamp =>
            {
                lastTimestamp = timestamp;
                return Task.CompletedTask;
            };

            var inboundMessage = new PongMessage
            {
                Timestamp = DateTime.Now
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            lastTimestamp.ShouldEqual(inboundMessage.Timestamp);
        }
    }

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