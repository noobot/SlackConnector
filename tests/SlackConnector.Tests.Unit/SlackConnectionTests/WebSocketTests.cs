using System;
using System.Threading.Tasks;
using Moq;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public class WebSocketTests
    {
        [Theory, AutoMoqData]
        private async Task should_detect_disconnect(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
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