using System;
using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Connections.Sockets;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class WebSocketTests
    {
        public class given_valid_setup_then_should_connect_to_expected_websocket_url : SpecsFor<SlackConnector>
        {
            private bool ConnectionChangedValue { get; set; }

            protected override void Given()
            {
                const string webSocketUrl = "im-a-websocket";

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(new SlackHandshake { WebSocketUrl = webSocketUrl });

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(webSocketUrl))
                    .Returns(GetMockFor<IWebSocketClient>().Object);
            }

            protected override void InitializeClassUnderTest()
            {
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object);
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

        public class when_socket_disconnects_given_valid_setup : SpecsFor<SlackConnector>
        {
            private Stack<bool> ConnectionChangedValue { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object);
            }

            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(new SlackHandshake());

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(It.IsAny<string>()))
                    .Returns(GetMockFor<IWebSocketClient>().Object);

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