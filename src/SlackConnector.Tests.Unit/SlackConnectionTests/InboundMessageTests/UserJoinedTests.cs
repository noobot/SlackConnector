using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class UserJoinedTests
    {
        [Test, AutoMoqData]
        public async Task should_raise_event(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
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
            slackConnection.UserCache[inboundMessage.User.Id].ShouldEqual(lastUser);
        }

        [Test, AutoMoqData]
        public async Task should_not_raise_event_given_missing_user_info(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
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
    }

    internal class given_exception_in_user_joined_event : BaseTest<UserJoinedMessage>
    {
        protected override void Given()
        {
            base.Given();

            SUT.OnUserJoined += slackUser =>
            {
                throw new NotImplementedException("THIS SHOULDN'T BUBBLE UP");
            };

            InboundMessage = new UserJoinedMessage
            {
                User = new User
                {
                    Id = "something"
                }
            };
        }

        protected override void When()
        {
            SUT.Initialise(ConnectionInfo).Wait();
        }

        [Test]
        public void then_shouldnt_bubble_exception()
        {
            Assert.DoesNotThrow(() =>
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnMessage += null, null, InboundMessage);
            });
        }
    }
}