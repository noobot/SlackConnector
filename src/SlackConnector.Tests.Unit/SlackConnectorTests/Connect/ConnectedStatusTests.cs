using System;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Tests.Unit.SlackConnectorTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class ConnectedStatusTests
    {
        public class given_valid_setup_when_connected : ValidSetup
        {
            protected override void When()
            {
                SUT.Connect("key").Wait();
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

            [Test]
            public void then_should_not_contain_connected_hubs()
            {
                SUT.ConnectedHubs.Count.ShouldEqual(0);
            }

            [Test]
            public void then_should_not_contain_users()
            {
                SUT.UserNameCache.Count.ShouldEqual(0);
            }
        }

        public class given_empty_api_key : ValidSetup
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
            }

            [Test]
            public void then_should_be_aware_of_current_state()
            {
                bool exceptionDetected = false;

                try
                {
                    SUT.Connect("").Wait();
                }
                catch (AggregateException ex)
                {
                    exceptionDetected = ex.InnerExceptions[0] is ArgumentNullException;
                }

                Assert.That(exceptionDetected, Is.True);
            }
        }
    }
}