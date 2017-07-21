using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Should;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class given_valid_connection_info
    {
        [Test, AutoMoqData]
        public void should_initialise_slack_connection(SlackConnection connection)
        {
            // given
            var info = new ConnectionInformation
            {
                Self = new ContactDetails { Id = "self-id" },
                Team = new ContactDetails { Id = "team-id" },
                Users = new Dictionary<string, SlackUser> { { "userid", new SlackUser() { Name = "userName" } } },
                SlackChatHubs = new Dictionary<string, SlackChatHub> { { "some-hub", new SlackChatHub() } },
                WebSocket = new Mock<IWebSocketClient>().Object
            };

            // when
            connection.Initialise(info).Wait();

            // then
            connection.Self.ShouldEqual(info.Self);
            connection.Team.ShouldEqual(info.Team);
            connection.UserCache.ShouldEqual(info.Users);
            connection.ConnectedHubs.ShouldEqual(info.SlackChatHubs);
            connection.ConnectedSince.HasValue.ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void should_be_connected_if_websocket_is_alive(Mock<IWebSocketClient> webSocketClient, SlackConnection connection)
        {
            // given
            var info = new ConnectionInformation
            {
                WebSocket = webSocketClient.Object
            };

            webSocketClient
                .Setup(x => x.IsAlive)
                .Returns(true);

            // when
            connection.Initialise(info).Wait();

            // then
            connection.IsConnected.ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public async Task should_initialise_ping_pong_monitor([Frozen]Mock<IMonitoringFactory> monitoringFactory, Mock<IPingPongMonitor> pingPongMonitor, SlackConnection connection)
        {
            // given
            var info = new ConnectionInformation
            {
                WebSocket = new Mock<IWebSocketClient>().Object
            };

            pingPongMonitor
                .Setup(x => x.StartMonitor(It.IsAny<Func<Task>>(), It.IsAny<Func<Task>>(), It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            monitoringFactory
                .Setup(x => x.CreatePingPongMonitor())
                .Returns(pingPongMonitor.Object);

            // when
            await connection.Initialise(info);

            // then
            pingPongMonitor.Verify(x => x.StartMonitor(It.IsNotNull<Func<Task>>(), It.IsNotNull<Func<Task>>(), TimeSpan.FromMinutes(2)), Times.Once);
        }
    }
}