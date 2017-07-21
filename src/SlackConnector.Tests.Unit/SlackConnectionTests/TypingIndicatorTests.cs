using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class TypingIndicatorTests
    {
        [Test, AutoMoqData]
        public async Task should_send_ping(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);
            var chatHub = new SlackChatHub { Id = "channelz-id" };

            // when
            await slackConnection.IndicateTyping(chatHub);

            // then
            webSocket.Verify(x => x.SendMessage(It.Is((TypingIndicatorMessage p) => p.Channel == chatHub.Id)));
        }
    }
}