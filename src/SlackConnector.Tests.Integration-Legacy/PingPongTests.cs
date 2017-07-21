using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SlackConnector.Tests.Integration
{
    public class PingPongTests : IntegrationTest
    {
        [Test]
        public async Task should_pong_to_our_ping()
        {
            // given
            bool hasPonged = false;
            SlackConnection.OnPong += timestamp => { hasPonged = true; return Task.CompletedTask;};

            // when
            await SlackConnection.Ping();

            // then
            for (int i = 0; i < 10; i++)
            {
                if (hasPonged)
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Assert.That(hasPonged, Is.True);
        }
    }
}