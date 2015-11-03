using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.SlackConnectorTests.Setups;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class InboundMessageTests
    {
        public class given_connector_is_setup_when_inbound_message_arrives : ValidSetup
        {
            private InboundMessage InboundMessage { get; set; }
            private bool MessageRaised { get; set; }
            private SlackMessage Result { get; set; }

            protected override void Given()
            {
                base.Given();

                var handshake = new SlackHandshake
                {
                    Users = new[]
                    {
                        new User
                        {
                            Id = "userABC",
                            Name = "I-have-a-name"
                        },
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(handshake);

                InboundMessage = new InboundMessage
                {
                    User = "userABC"
                };

                SUT.OnMessageReceived += async message =>
                {
                    Result = message;
                    MessageRaised = true;
                    await Task.Factory.StartNew(() => { });
                };

                SUT.Connect("blah").Wait();
            }

            protected override void When()
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnMessage += null, null, InboundMessage);
            }

            [Test]
            public void then_should_raise_event()
            {
                MessageRaised.ShouldBeTrue();
            }

            [Test]
            public void then_should_pass_through_expected_message()
            {
                var expected = new SlackMessage
                {
                    User = new SlackUser
                    {
                        Id = "userABC",
                        Name = "I-have-a-name"
                    }
                };

                Result.ShouldLookLike(expected);
            }

        }
    }
}