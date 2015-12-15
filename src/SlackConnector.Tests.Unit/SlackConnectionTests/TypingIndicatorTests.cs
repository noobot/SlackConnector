using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class TypingIndicatorTests
    {
        internal class given_valid_bot_message : SpecsFor<SlackConnection>
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
                SUT.IndicateTyping(_chatHub).Wait();
            }

            [Test]
            public void then_should_pass_expected_message_object()
            {
                var typingMessage = _webSocketClient.SendMessage_Message as TypingIndicatorMessage;
                typingMessage.ShouldNotBeNull();
                typingMessage.Channel.ShouldEqual(_chatHub.Id);
                typingMessage.Type.ShouldEqual("typing");
            }
        }

    }
}