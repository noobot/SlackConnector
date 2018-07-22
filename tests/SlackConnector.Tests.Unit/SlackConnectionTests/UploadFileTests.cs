using System.IO;
using System.Threading.Tasks;
using Moq;
using AutoFixture.Xunit2;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public class UploadFileTests
    {
        [Theory, AutoMoqData]
        private async Task should_upload_file_from_disk(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IFileClient> fileClient, 
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string filePath = "expected-file-name";
            var chatHub = new SlackChatHub { Id = "channelz-id" };

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateFileClient())
                .Returns(fileClient.Object);

            // when
            await slackConnection.Upload(chatHub, filePath);

            // then
            fileClient
                .Verify(x => x.PostFile(slackKey, chatHub.Id, filePath), Times.Once);
        }

        [Theory, AutoMoqData]
        private async Task should_upload_file_from_stream(
            [Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IFileClient> fileClient, 
            Mock<IWebSocketClient> webSocket, 
            SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";
            const string fileName = "expected-file-name";
            var chatHub = new SlackChatHub { Id = "channelz-id" };
            var stream = new MemoryStream();

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateFileClient())
                .Returns(fileClient.Object);

            // when
            await slackConnection.Upload(chatHub, stream, fileName);

            // then
            fileClient
                .Verify(x => x.PostFile(slackKey, chatHub.Id, stream, fileName), Times.Once);
        }
    }
}