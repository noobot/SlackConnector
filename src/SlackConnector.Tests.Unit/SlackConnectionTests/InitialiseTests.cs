using System.Collections.Generic;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class InitialiseTests
    {
        internal class given_valid_connection_info : SpecsFor<SlackConnection>
        {
            private ConnectionInformation Info { get; set; }

            protected override void Given()
            {
                Info = new ConnectionInformation
                {
                    Self = new ContactDetails { Id = "self-id" },
                    Team = new ContactDetails { Id = "team-id" },
                    Users = new Dictionary<string, string> { { "userid", "userName" } },
                    SlackChatHubs = new Dictionary<string, SlackChatHub> { { "some-hub", new SlackChatHub() } },
                    WebSocket = GetMockFor<IWebSocketClient>().Object
                };
            }

            protected override void When()
            {
                SUT.Initialise(Info);
            }

            [Test]
            public void then_should_populate_self()
            {
                SUT.Self.ShouldEqual(Info.Self);
            }

            [Test]
            public void then_should_populate_team()
            {
                SUT.Team.ShouldEqual(Info.Team);
            }

            [Test]
            public void then_should_populate_users()
            {
                SUT.UserNameCache.ShouldEqual(Info.Users);
            }

            [Test]
            public void then_should_slack_hubs()
            {
                SUT.ConnectedHubs.ShouldEqual(Info.SlackChatHubs);
            }

            [Test]
            public void then_should_detect_as_connected()
            {
                SUT.IsConnected.ShouldBeTrue();
                SUT.ConnectedSince.HasValue.ShouldBeTrue();
            }
        }
    }
}