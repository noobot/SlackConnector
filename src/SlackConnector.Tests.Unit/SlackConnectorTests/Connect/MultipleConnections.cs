using System;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Exceptions;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class MultipleConnections
    {
        public class given_connector_is_already_connected_when_calling_connect : SpecsFor<SlackConnector>
        {
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

                SUT.Connect("something").Wait();
            }

            [Test]
            public void then_should_throw_exception()
            {
                bool exceptionDetected = false;

                try
                {
                    SUT.Connect("").Wait();
                }
                catch (AggregateException ex)
                {
                    exceptionDetected = ex.InnerExceptions[0] is AlreadyConnectedException;
                }

                Assert.That(exceptionDetected, Is.True);
            }
        }
    }
}