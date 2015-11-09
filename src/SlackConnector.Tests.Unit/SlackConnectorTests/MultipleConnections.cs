using System;
using NUnit.Framework;
using SlackConnector.Exceptions;
using SlackConnector.Tests.Unit.SlackConnectionTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class MultipleConnections
    {
        public class given_connector_is_already_connected_when_calling_connect : SlackConnectorIsSetup
        {
            protected override void Given()
            {
                base.Given();
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