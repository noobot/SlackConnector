using NUnit.Framework;
using Should;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class PingTests
    {
        internal class given_valid_connection_when_initiating_a_ping : SpecsFor<SlackConnection>
        {
            private string SlackKey = "doobeedoo";
            private SlackChatHub _chatHub;
            private WebSocketClientStub _webSocketClient;

            protected override void Given()
            {
                _webSocketClient = new WebSocketClientStub();
                _chatHub = new SlackChatHub { Id = "channelz-id" };

                var connectionInfo = new ConnectionInformation
                {
                    SlackKey = SlackKey,
                    WebSocket = _webSocketClient
                };
                SUT.Initialise(connectionInfo);
            }

            protected override void When()
            {
                SUT.Ping().Wait();
            }

            [Test]
            public void then_should_send_ping_message_with_expected_type()
            {
                var typingMessage = _webSocketClient.SendMessage_Message as PingMessage;
                typingMessage.ShouldNotBeNull();

                typingMessage.Type.ShouldEqual("ping");
            }
        }
    }
}