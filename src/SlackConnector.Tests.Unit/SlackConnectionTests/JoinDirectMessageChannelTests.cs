using System;
using System.Runtime.InteropServices;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Models;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class JoinDirectMessageChannel
    {
        internal class given_dm_channel : SpecsFor<SlackConnection>
        {
            private readonly string SlackKey = "doobeedoo";
            private readonly string UserId = "something interesting";
            private readonly Channel ReturnChannel = new Channel { Id = "super-id", Name = "dm-channel" };
            private SlackChatHub Result { get; set; }

            protected override void Given()
            {
                var connectionInfo = new ConnectionInformation { SlackKey = SlackKey,  WebSocket = new WebSocketClientStub()};
                SUT.Initialise(connectionInfo);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateChannelMessenger())
                    .Returns(GetMockFor<IChannelClient>().Object);

                GetMockFor<IChannelClient>()
                    .Setup(x => x.JoinDirectMessageChannel(SlackKey, UserId))
                    .ReturnsAsync(ReturnChannel);
            }

            protected override void When()
            {
                Result = SUT.JoinDirectMessageChannel(UserId).Result;
            }

            [Test]
            public void then_should_return_expected_slack_hub()
            {
                var expected = new SlackChatHub
                {
                    Id = ReturnChannel.Id,
                    Name = ReturnChannel.Name,
                    Type = SlackChatHubType.DM
                };
                Result.ShouldLookLike(expected);
            }
        }

        internal class given_no_valid_user_id : SpecsFor<SlackConnection>
        {
            [Test]
            public void should_throw_exception_when_no_chathub_given()
            {
                ArgumentNullException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel(null).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as ArgumentNullException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: user"));
            }

            [Test]
            public void should_throw_exception_when_no_chathub_id_given()
            {
                ArgumentNullException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel("").Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as ArgumentNullException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: user"));
            }
        }
    }
}