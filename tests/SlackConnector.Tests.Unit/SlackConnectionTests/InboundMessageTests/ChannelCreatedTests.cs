using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Ploeh.AutoFixture;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    public class ChannelCreatedTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection,
            SlackUser slackUser,
            Fixture fixture)
        {
            // given
            var connectionInfo = new ConnectionInformation
            {
                WebSocket = webSocket.Object,
                Users = new Dictionary<string, SlackUser>
                {
                    {slackUser.Id , slackUser}
                }
            };
            await slackConnection.Initialise(connectionInfo);

            SlackChannelCreated channelCreated = null;
            slackConnection.OnChannelCreated += channel =>
            {
                channelCreated = channel;
                return Task.CompletedTask;
            };

            var inboundMessage = new ChannelCreatedMessage
            {
                Channel = new Channel
                {
                    Creator = slackUser.Id,
                    Id = fixture.Create<string>(),
                    Name = fixture.Create<string>()
                }
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            channelCreated.Id.ShouldBe(inboundMessage.Channel.Id);
            channelCreated.Name.ShouldBe(inboundMessage.Channel.Name);
            channelCreated.Creator.ShouldBe(slackUser);
            slackConnection.ConnectedHubs.ContainsKey(inboundMessage.Channel.Id).ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_event_given_missing_data(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            SlackChannelCreated channelCreated = null;
            slackConnection.OnChannelCreated += channel =>
            {
                channelCreated = channel;
                return Task.CompletedTask;
            };

            var inboundMessage = new ChannelCreatedMessage { Channel = null };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            channelCreated.ShouldBeNull();
            slackConnection.ConnectedHubs.ShouldBeEmpty();
        }
    }
}