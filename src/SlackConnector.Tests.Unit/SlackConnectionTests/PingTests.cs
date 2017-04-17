using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class PingTests
    {
        [Test, AutoMoqData]
        public async Task should_return_expected_slack_hub([Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string userId = "some-id";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateChannelClient())
                .Returns(channelClient.Object);

            var returnChannel = new Channel { Id = "super-id", Name = "dm-channel" };
            channelClient
                .Setup(x => x.JoinDirectMessageChannel(slackKey, userId))
                .ReturnsAsync(returnChannel);

            // when
            await slackConnection.Ping();

            // then
            webSocket.Verify(x => x.SendMessage(It.IsAny<PingMessage>()));
        }
    }
}