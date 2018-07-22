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
    public class SetChannelTopicTests
    {
        [Theory, AutoMoqData]
        private async Task should_return_expected_slack_topic(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, 
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string channelName = "public-channel-name";
            const string channelTopic = "new topic";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateChannelClient())
                .Returns(channelClient.Object);

            channelClient
                .Setup(x => x.SetTopic(slackKey, channelName, channelTopic))
                .ReturnsAsync(channelTopic);

            // when
            var result = await slackConnection.SetChannelTopic(channelName, channelTopic);

            // then
            result.ShouldLookLike(new SlackTopic
            {
                ChannelName = channelName,
                Topic = channelTopic
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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelTopic(null, "topic"));

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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelTopic(string.Empty, "topic"));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: channelName");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_null_topic(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelTopic("channel", null));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: topic");
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_given_empty_topic(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => slackConnection.SetChannelTopic("channel", string.Empty));

            // then
            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: topic");
        }
    }
}