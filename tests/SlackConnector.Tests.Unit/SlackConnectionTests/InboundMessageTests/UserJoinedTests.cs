using System;
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
    public class UserJoinedTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            SlackUser lastUser = null;
            slackConnection.OnUserJoined += user =>
            {
                lastUser = user;
                return Task.CompletedTask;
            };

            var inboundMessage = new UserJoinedMessage
            {
                MessageType = MessageType.Team_Join,
                User = new User
                {
                    Id = "some-id",
                    Name = "my-name",
                    TimeZoneOffset = -231,
                    IsBot = true,
                    Presence = "active",
                    Profile = new Profile
                    {
                        Email = "some-email@mail.com"
                    }
                }
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            lastUser.ShouldLookLike(new SlackUser
            {
                Id = "some-id",
                Name = "my-name",
                TimeZoneOffset = -231,
                IsBot = true,
                Online = true,
                Email = "some-email@mail.com"
            });

            slackConnection.UserCache.ContainsKey(inboundMessage.User.Id).ShouldBeTrue();
            slackConnection.UserCache[inboundMessage.User.Id].ShouldBe(lastUser);
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_event_given_missing_user_info(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            SlackUser lastUser = null;
            slackConnection.OnUserJoined += user =>
            {
                lastUser = user;
                return Task.CompletedTask;
            };

            var inboundMessage = new UserJoinedMessage
            {
                User = new User
                {
                    Id = null
                }
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            lastUser.ShouldBeNull();
            slackConnection.UserCache.ShouldBeEmpty();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_exception(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);
            slackConnection.OnUserJoined += user => throw new NotImplementedException("THIS SHOULDN'T BUBBLE UP");

            var inboundMessage = new UserJoinedMessage
            {
                User = new User
                {
                    Id = null
                }
            };

            // when & then (does not throw)
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);
        }
    }
}