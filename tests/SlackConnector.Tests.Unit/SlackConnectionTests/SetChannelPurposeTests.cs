using System;
using System.Threading.Tasks;
using Moq;
using AutoFixture.Xunit2;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public class SetChannelPurposeTests
    {
        [Theory, AutoMoqData]
        private async Task should_return_expected_slack_purpose(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, 
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string channelName = "public-channel-name";
            const string channelPurpose = "new purpose";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateChannelClient())
                .Returns(channelClient.Object);

            channelClient
                .Setup(x => x.SetPurpose(slackKey, channelName, channelPurpose))
                .ReturnsAsync(channelPurpose);

            // when
            var result = await slackConnection.SetChannelPurpose(channelName, channelPurpose);

            // then
            result.ShouldLookLike(new SlackPurpose
            {
                ChannelName = channelName,
                Purpose = channelPurpose
            });
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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelPurpose(null, "purpose"));

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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelPurpose(string.Empty, "purpose"));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: channelName");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_null_purpose(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelPurpose("channel", null));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: purpose");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_empty_purpose(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelPurpose("channel", string.Empty));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: purpose");
        }
    }
}