using System.Threading.Tasks;
using Moq;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    public class DmJoinedTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);
            
            SlackChatHub lastHub = null;
            slackConnection.OnChatHubJoined += hub =>
            {
                lastHub = hub;
                return Task.CompletedTask;
            };

            var inboundMessage = new DmChannelJoinedMessage
            {
                Channel = new Im
                {
                    User = "test-user",
                    Id = "channel-id",
                    IsIm = true,
                    IsOpen = true
                }
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            slackConnection.ConnectedHubs.ContainsKey("channel-id").ShouldBeTrue();
            slackConnection.ConnectedHubs["channel-id"].ShouldBe(lastHub);
            lastHub.ShouldLookLike(new SlackChatHub
            {
                Id = "channel-id",
                Name = "@test-user",
                Type = SlackChatHubType.DM
            });
        }
    }
}