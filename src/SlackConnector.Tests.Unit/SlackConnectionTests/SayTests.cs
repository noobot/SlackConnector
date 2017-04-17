using System;
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
using SpecsFor;

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

        internal class given_no_valid_chathub_id : SpecsFor<SlackConnection>
        {
            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateChatClient())
                    .Returns(GetMockFor<IChatClient>().Object);
            }

            [Test]
            public void should_throw_exception_when_no_chathub_given()
            {
                MissingChannelException exception = null;

                try
                {
                    SUT.Say(new BotMessage { ChatHub = null }).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as MissingChannelException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("When calling the Say() method, the message parameter must have its ChatHub property set."));
            }

            [Test]
            public void should_throw_exception_when_no_chathub_id_given()
            {
                MissingChannelException exception = null;

                try
                {
                    SUT.Say(new BotMessage { ChatHub = new SlackChatHub { Id = "" } }).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as MissingChannelException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("When calling the Say() method, the message parameter must have its ChatHub property set."));
            }
        }
    }
}