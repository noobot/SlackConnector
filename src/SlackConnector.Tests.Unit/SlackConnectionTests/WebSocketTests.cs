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
            private Stack<bool> ConnectionChangedValue { get; set; }

            protected override void Given()
            {
                base.Given();
                ConnectionChangedValue = new Stack<bool>();

                SUT.OnConnectionStatusChanged += connected => ConnectionChangedValue.Push(connected);

                var info = new ConnectionInformation { WebSocket = GetMockFor<IWebSocketClient>().Object };
                SUT.Initialise(info);
            }

            protected override void When()
            {
                GetMockFor<IWebSocketClient>()
                    .Raise(x => x.OnClose += null, new EventArgs());
            }

            [Test]
            public void then_should_detect_diconnect()
            {
                ConnectionChangedValue.Count.ShouldEqual(1);
                ConnectionChangedValue.Pop().ShouldBeFalse();
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