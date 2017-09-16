using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Integration
{
    public class PingPongTests : IntegrationTest
    {
        [Fact]
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

            hasPonged.ShouldBeTrue();
        }
    }
}