using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Sockets;
using SlackConnector.Tests.Unit.SlackConnectionTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorFactoryTests
{
    public static class WebSocketTests
    {
        public class given_valid_setup_then_should_connect_to_expected_websocket_url : SlackConnectorIsSetup
        {
            private bool ConnectionChangedValue { get; set; }

            protected override void Given()
            {
                const string webSocketUrl = "im-a-websocket";
                base.Given();

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(new SlackHandshake { WebSocketUrl = webSocketUrl, Ok = true });

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(webSocketUrl))
                    .Returns(GetMockFor<IWebSocketClient>().Object);
            }

            protected override void When()
            {
                SUT.OnConnectionStatusChanged += connected => ConnectionChangedValue = connected;
                SUT.Connect("yay").Wait();
            }

            [Test]
            public void then_should_connect()
            {
                SUT.IsConnected.ShouldBeTrue();
                SUT.ConnectedSince.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            }

            [Test]
            public void then_should_invoke_connect_changed_event()
            {
                ConnectionChangedValue.ShouldBeTrue();
            }
        }

        public class when_socket_disconnects_given_valid_setup : SlackConnectorIsSetup
        {
            private Stack<bool> ConnectionChangedValue { get; set; }

            protected override void Given()
            {
                base.Given();
                ConnectionChangedValue = new Stack<bool>();

                SUT.OnConnectionStatusChanged += connected => ConnectionChangedValue.Push(connected);
                SUT.Connect("yay").Wait();
            }

            protected override void When()
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnClose += null, new EventArgs());
            }

            [Test]
            public void then_should_detect_diconnect()
            {
                ConnectionChangedValue.Count.ShouldEqual(2);
                ConnectionChangedValue.Pop().ShouldBeFalse();
                ConnectionChangedValue.Pop().ShouldBeTrue();
            }

            [Test]
            public void then_should_not_detect_as_connected()
            {
                SUT.IsConnected.ShouldBeFalse();
                SUT.ConnectedSince.ShouldBeNull();
            }
        }
    }
}