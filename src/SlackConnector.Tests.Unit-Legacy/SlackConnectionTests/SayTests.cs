using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    internal class SayTests
    {
        [Test, AutoMoqData]
        public async Task should_send_message([Frozen]Mock<IConnectionFactory> connectionFactory,
            Mock<IChatClient> chatClient, Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            const string slackKey = "key-yay";

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            connectionFactory
                .Setup(x => x.CreateChatClient())
                .Returns(chatClient.Object);

            var message = new BotMessage
            {
                Text = "some text",
                ChatHub = new SlackChatHub { Id = "channel-id" },
                Attachments = new List<SlackAttachment>()
            };

            // when
            await slackConnection.Say(message);

            // then
            chatClient
                .Verify(x => x.PostMessage(slackKey, message.ChatHub.Id, message.Text, message.Attachments), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task should_throw_exception_given_null_chat_hub(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = Assert.ThrowsAsync<MissingChannelException>(() => slackConnection.Say(new BotMessage { ChatHub = null }));

            // then
            Assert.That(exception.Message, Is.EqualTo("When calling the Say() method, the message parameter must have its ChatHub property set."));
        }

        [Test, AutoMoqData]
        public async Task should_throw_exception_given_empty_chat_hub_id(Mock<IWebSocketClient> webSocket, SlackConnection slackConnection)
        {
            // given
            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = Assert.ThrowsAsync<MissingChannelException>(() => slackConnection.Say(new BotMessage { ChatHub = new SlackChatHub { Id = string.Empty } }));

            // then
            Assert.That(exception.Message, Is.EqualTo("When calling the Say() method, the message parameter must have its ChatHub property set."));
        }
    }
}