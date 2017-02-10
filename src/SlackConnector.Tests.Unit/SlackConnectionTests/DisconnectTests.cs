using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class DisconnectTests
    {
        internal class given_connection_established : SpecsFor<SlackConnection>
        {
            private ConnectionInformation Info { get; set; }

            protected override void Given()
            {
                Info = new ConnectionInformation
                {
                    Self = new ContactDetails { Id = "self-id" },
                    Team = new ContactDetails { Id = "team-id" },
                    Users = new Dictionary<string, SlackUser> { { "userid", new SlackUser() { Name = "userName" } } },
                    SlackChatHubs = new Dictionary<string, SlackChatHub> { { "some-hub", new SlackChatHub() } },
                    WebSocket = GetMockFor<IWebSocketClient>().Object
                };

                GetMockFor<IWebSocketClient>()
                    .Setup(x => x.IsAlive)
                    .Returns(true);

                SUT.Initialise(Info);
            }

            protected override void When()
            {
                SUT.Close().Wait();
            }

            [Test]
            public void then_should_populate_self()
            {
                GetMockFor<IWebSocketClient>()
                    .Verify(x => x.Close(), Times.Once);
            }
        }

        internal class given_connection_not_active : SpecsFor<SlackConnection>
        {
            private ConnectionInformation Info { get; set; }

            protected override void Given()
            {
                Info = new ConnectionInformation
                {
                    Self = new ContactDetails { Id = "self-id" },
                    Team = new ContactDetails { Id = "team-id" },
                    Users = new Dictionary<string, SlackUser> { { "userid", new SlackUser() { Name = "userName" } } },
                    SlackChatHubs = new Dictionary<string, SlackChatHub> { { "some-hub", new SlackChatHub() } },
                    WebSocket = GetMockFor<IWebSocketClient>().Object
                };

                GetMockFor<IWebSocketClient>()
                    .Setup(x => x.IsAlive)
                    .Returns(false);

                SUT.Initialise(Info);
            }

            protected override void When()
            {
                SUT.Close().Wait();
            }

            [Test]
            public void then_should_populate_self()
            {
                GetMockFor<IWebSocketClient>()
                    .Verify(x => x.Close(), Times.Never);
            }
        }
    }
}