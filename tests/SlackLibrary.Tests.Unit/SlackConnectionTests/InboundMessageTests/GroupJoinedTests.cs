using System.Threading.Tasks;
using Moq;
using SlackLibrary.Connections.Models;
using SlackLibrary.Connections.Sockets;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Models;
using Xunit;
using Shouldly;

namespace SlackLibrary.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    public class GroupJoinedTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            const string hubId = "this-is-the-id";
            SlackChatHub lastHub = null;
            slackConnection.OnChatHubJoined += hub =>
            {
                lastHub = hub;
                return Task.CompletedTask;
            };

            var inboundMessage = new GroupJoinedMessage
            {
                Channel = new Group { Id = hubId }
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            lastHub.Id.ShouldBe(hubId);
            lastHub.Type.ShouldBe(SlackChatHubType.Group);
            slackConnection.ConnectedHubs.ContainsKey(hubId).ShouldBeTrue();
            slackConnection.ConnectedHubs[hubId].ShouldBe(lastHub);
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_event_given_missing_data(
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

            var inboundMessage = new GroupJoinedMessage { Channel = null };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            lastHub.ShouldBeNull();
            slackConnection.ConnectedHubs.ShouldBeEmpty();
        }
    }
}