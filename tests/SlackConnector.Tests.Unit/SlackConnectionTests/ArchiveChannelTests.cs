using System;
using System.Threading.Tasks;
using Moq;
using AutoFixture.Xunit2;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public class ArchiveChannelTests
    {
        [Theory, AutoMoqData]
        private async Task should_be_successful(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, 
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string channelName = "public-channel-name";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateChannelClient())
                .Returns(channelClient.Object);

            // when
             await slackConnection.ArchiveChannel(channelName);

            // then
            channelClient.Verify(x => x.ArchiveChannel(slackKey, channelName), Times.Once);
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_null_channel_name(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.ArchiveChannel(null));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: channelName");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_empty_channel_name(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.ArchiveChannel(string.Empty));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: channelName");
        }
    }
}