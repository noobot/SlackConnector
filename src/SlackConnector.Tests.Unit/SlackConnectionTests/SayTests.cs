using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class SayTests
    {
        internal class given_valid_bot_message : SpecsFor<SlackConnection>
        {
            private string SlackKey = "doobeedoo";
            private BotMessage Message { get; set; }

            protected override void Given()
            {
                Message = new BotMessage
                {
                    Text = "some text",
                    ChatHub = new SlackChatHub { Id = "channel-id" },
                    Attachments = new List<SlackAttachment>()
                };

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateChatClient())
                    .Returns(GetMockFor<IChatClient>().Object);

                var connectionInfo = new ConnectionInformation
                {
                    SlackKey = SlackKey,
                    WebSocket = GetMockFor<IWebSocketClient>().Object
                };

                SUT.Initialise(connectionInfo);
            }

            protected override void When()
            {
                SUT.Say(Message).Wait();
            }

            [Test]
            public void then_should_call_messenger()
            {
                GetMockFor<IChatClient>()
                    .Verify(x => x.PostMessage(SlackKey, Message.ChatHub.Id, Message.Text, Message.Attachments), Times.Once);
            }
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