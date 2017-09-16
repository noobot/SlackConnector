using System;
using System.Threading.Tasks;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    public class PongTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
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
            lastTimestamp.ShouldBe(inboundMessage.Timestamp);
        }

        [Theory, AutoMoqData]
        private async Task should_pong_monitor(
            [Frozen]Mock<IMonitoringFactory> monitoringFactory, 
            Mock<IPingPongMonitor> pingPongMonitor,
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            monitoringFactory
                .Setup(x => x.CreatePingPongMonitor())
                .Returns(pingPongMonitor.Object);

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            var inboundMessage = new PongMessage
            {
                Timestamp = DateTime.Now
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            pingPongMonitor.Verify(x => x.Pong(), Times.Once);
        }
    }
}