using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class JoinDirectMessageChannelTests
    {
        [Test, AutoMoqData]
        public async Task should_return_expected_slack_hub([Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChannelClient> channelClient, Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string userId = "some-id";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey};
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

        internal class given_no_valid_user_id : SpecsFor<SlackConnection>
        {
            [Test]
            public void should_throw_exception_when_no_chathub_given()
            {
                ArgumentNullException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel(null).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as ArgumentNullException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: user"));
            }

            [Test]
            public void should_throw_exception_when_no_chathub_id_given()
            {
                ArgumentNullException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel("").Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as ArgumentNullException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: user"));
            }
        }
    }
}