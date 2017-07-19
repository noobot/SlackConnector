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
    internal class GroupJoinedTests
    {
        [Test, AutoMoqData]
        public async Task should_raise_event(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
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
            lastHub.Id.ShouldEqual(hubId);
            lastHub.Type.ShouldEqual(SlackChatHubType.Group);
            slackConnection.ConnectedHubs.ContainsKey(hubId).ShouldBeTrue();
            slackConnection.ConnectedHubs[hubId].ShouldEqual(lastHub);
        }

        [Test, AutoMoqData]
        public async Task should_not_raise_event_given_missing_data(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
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