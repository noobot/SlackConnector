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
    public class JoinDirectMessageChannelTests
    {
        [Theory, AutoMoqData]
        private async Task should_return_expected_slack_hub(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, 
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
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
            var result = await slackConnection.JoinDirectMessageChannel(userId);

            // then
            result.ShouldLookLike(new SlackChatHub
            {
                Id = returnChannel.Id,
                Name = returnChannel.Name,
                Type = SlackChatHubType.DM
            });
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_null_user_id(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.JoinDirectMessageChannel(null));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: user");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_empty_user_id(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.JoinDirectMessageChannel(string.Empty));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: user");
        }
    }
}