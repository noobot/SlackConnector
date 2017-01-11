using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class given_new_user_joins_team : BaseTest<UserJoinedMessage>
    {
        private SlackUser _expectedUser;
        private SlackUser _lastUser;

        protected override void Given()
        {
            base.Given();

            SUT.OnUserJoined += user =>
            {
                _lastUser = user;
                return Task.FromResult(false);
            };

            InboundMessage = new UserJoinedMessage
            {
                MessageType = MessageType.Team_Join,
                User = new User
                {
                    Id = "some-id",
                    Name = "my-name",
                    TimeZoneOffset = -231,
                    IsBot = true,
                    Presence = "active"
                }
            };

            _expectedUser = new SlackUser
            {
                Id = "some-id",
                Name = "my-name",
                TimeZoneOffset = -231,
                IsBot = true,
                Online = true
            };
        }

        [Test]
        public void then_should_raise_event_with_user_info()
        {
            _lastUser.ShouldLookLike(_expectedUser);
        }

        [Test]
        public void then_should_add_add_user_to_user_cache()
        {
            SUT.UserCache.ContainsKey(InboundMessage.User.Id).ShouldBeTrue();
            SUT.UserCache[InboundMessage.User.Id].ShouldEqual(_lastUser);
        }
    }

    internal class given_missing_user_id_when_new_user_joins_team : BaseTest<UserJoinedMessage>
    {
        private SlackUser _lastUser;

        protected override void Given()
        {
            base.Given();

            SUT.OnUserJoined += slackUser =>
            {
                _lastUser = slackUser;
                return Task.FromResult(false);
            };

            InboundMessage = new UserJoinedMessage
            {
                User = new User
                {
                    Id = null
                }
            };
        }

        [Test]
        public void then_should_not_raise_evnet()
        {
            _lastUser.ShouldBeNull();
        }

        [Test]
        public void then_should_not_modify_connect_hubs()
        {
            SUT.UserCache.ShouldBeEmpty();
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
            SUT.Initialise(ConnectionInfo);
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