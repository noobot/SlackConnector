using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class WebSocketTests
    {
        [Test, AutoMoqData]
        public async Task should_detect_disconnect(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            bool connectionChangedValue = false;
            slackConnection.OnDisconnect += () => connectionChangedValue = true;

            var info = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(info);

            // when
            webSocket.Raise(x => x.OnClose += null, new EventArgs());

            // then
            connectionChangedValue.ShouldBeTrue();
            slackConnection.IsConnected.ShouldBeFalse();
            slackConnection.ConnectedSince.ShouldBeNull();
        }
    }
}