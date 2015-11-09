using System;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Exceptions;
using SlackConnector.Tests.Unit.SlackConnectorTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorFactoryTests
{
    public static class ConnectedStatusTests
    {
        public class given_valid_setup_when_connected : SlackConnectorIsSetup
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
                SUT.ConnectedSince.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
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

        public class given_handshake_was_not_ok : SlackConnectorIsSetup
        {
            private SlackHandshake HandshakeResponse { get; set; }

            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                HandshakeResponse = new SlackHandshake { Ok = false, Error = "I AM A ERROR" };
                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(HandshakeResponse);
            }

            [Test]
            public void then_should_throw_exception()
            {
                HandshakeException exception = null;

                try
                {
                    SUT.Connect("something").Wait();
                }
                catch (AggregateException ex)
                {

                    exception = ex.InnerExceptions[0] as HandshakeException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo(HandshakeResponse.Error));
            }
        }

        public class given_empty_api_key : SlackConnectorIsSetup
        {
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