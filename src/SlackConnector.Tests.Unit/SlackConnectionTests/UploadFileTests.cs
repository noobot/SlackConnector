using System.IO;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class given_valid_connection_when_uploading_file_from_disk : SpecsFor<SlackConnection>
    {
        private string SlackKey = "doobeedoo";
        private SlackChatHub _chatHub;
        private WebSocketClientStub _webSocketClient;
        private string _filePath = "expected-file-name";

        protected override void Given()
        {
            _webSocketClient = new WebSocketClientStub();
            _chatHub = new SlackChatHub { Id = "channelz-id" };

            var connectionInfo = new ConnectionInformation
            {
                SlackKey = SlackKey,
                WebSocket = _webSocketClient
            };

            GetMockFor<IConnectionFactory>()
                .Setup(x => x.CreateFileClient())
                .Returns(GetMockFor<IFileClient>().Object);

            SUT.Initialise(connectionInfo);
        }

        protected override void When()
        {
            SUT.Upload(_chatHub, _filePath).Wait();
        }

        [Test]
        public void then_should_call_client()
        {
            GetMockFor<IFileClient>()
                .Verify(x => x.PostFile(SlackKey, _chatHub.Id, _filePath), Times.Once);
        }
    }

    internal class given_valid_connection_when_uploading_stream : SpecsFor<SlackConnection>
    {
        private string SlackKey = "doobeedoo";
        private SlackChatHub _chatHub;
        private WebSocketClientStub _webSocketClient;
        private readonly string _filePath = "expected-file-name";
        private readonly Stream _stream = new MemoryStream();

        protected override void Given()
        {
            _webSocketClient = new WebSocketClientStub();
            _chatHub = new SlackChatHub { Id = "channelz-id" };

            var connectionInfo = new ConnectionInformation
            {
                SlackKey = SlackKey,
                WebSocket = _webSocketClient
            };

            GetMockFor<IConnectionFactory>()
                .Setup(x => x.CreateFileClient())
                .Returns(GetMockFor<IFileClient>().Object);

            SUT.Initialise(connectionInfo);
        }

        protected override void When()
        {
            SUT.Upload(_chatHub, _stream, _filePath).Wait();
        }

        [Test]
        public void then_should_call_client()
        {
            GetMockFor<IFileClient>()
                .Verify(x => x.PostFile(SlackKey, _chatHub.Id, _stream, _filePath), Times.Once);
        }
    }
}