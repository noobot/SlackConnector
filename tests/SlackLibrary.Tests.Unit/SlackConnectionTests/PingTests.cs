using System.Threading.Tasks;
using Moq;
using SlackLibrary.Connections.Sockets;
using SlackLibrary.Connections.Sockets.Messages.Outbound;
using SlackLibrary.Models;
using Xunit;

namespace SlackLibrary.Tests.Unit.SlackConnectionTests
{
    public class PingTests
    {
        [Theory, AutoMoqData]
        private async Task should_send_ping(
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);
            
            // when
            await slackConnection.Ping();

            // then
            webSocket.Verify(x => x.SendMessage(It.IsAny<PingMessage>()));
        }
    }
}