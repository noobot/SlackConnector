using System;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class ConnectedStatusTests
    {
        public class given_valid_setup_when_connected : SpecsFor<SlackConnector>
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
                    .ReturnsAsync(Responses.ValidHandshake());
            }

            protected override void When()
            {
                SUT.Connect("").Wait();
            }

            [Test]
            public void then_should_be_aware_of_current_state()
            {
                SUT.IsConnected.ShouldBeTrue();
            }

            [Test]
            public void then_should_have_a_connected_since_date()
            {
                SUT.ConnectedSince.ShouldBeGreaterThanOrEqualTo(DateTime.Now.AddSeconds(-1));
                SUT.ConnectedSince.ShouldBeLessThan(DateTime.Now);
            }
        }
    }
}