using System;
using System.Collections.Generic;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class WebSocketTests
    {
        internal class when_socket_disconnects_given_valid_setup : SpecsFor<SlackConnection>
        {
            private bool ConnectionChangedValue { get; set; }

            protected override void Given()
            {
                base.Given();
                SUT.OnDisconnect += () => ConnectionChangedValue = true;

                var info = new ConnectionInformation { WebSocket = GetMockFor<IWebSocketClient>().Object };
                SUT.Initialise(info).Wait();
            }

            protected override void When()
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnClose += null, new EventArgs());
            }

            [Test]
            public void then_should_detect_diconnect()
            {
                ConnectionChangedValue.ShouldBeTrue();
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