using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Messaging;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.SlackConnectorTests.Setups;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class SayTests
    {
        public class given_valid_bot_message : SlackConnectorIsSetup
        {
            private string SlackKey = "doobeedoo";
            private BotMessage Message { get; set; }

            protected override void Given()
            {
                base.Given();
                SUT.Connect(SlackKey).Wait();

                Message = new BotMessage
                {
                    Text = "some text",
                    ChatHub = new SlackChatHub { Id = "channel-id" },
                    Attachments = new List<SlackAttachment>()
                };

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateChatMessenger())
                    .Returns(GetMockFor<IChatMessenger>().Object);
            }

            protected override void When()
            {
                SUT.Say(Message).Wait();
            }

            [Test]
            public void then_should_call_messenger()
            {
                GetMockFor<IChatMessenger>()
                    .Verify(x => x.PostMessage(SlackKey, Message.ChatHub.Id, Message.Text, Message.Attachments), Times.Once);
            }
        }

        public class given_no_valid_chathub_id : SlackConnectorIsSetup
        {
            protected override void Given()
            {
                base.Given();
                SUT.Connect("something").Wait();

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateChatMessenger())
                    .Returns(GetMockFor<IChatMessenger>().Object);
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