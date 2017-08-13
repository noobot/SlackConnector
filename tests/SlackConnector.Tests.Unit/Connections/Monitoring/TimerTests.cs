using System;
using System.Threading;
using Xunit;
using Should;
using Timer = SlackConnector.Connections.Monitoring.Timer;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    public class TimerTests
    {
        [Fact]
        public void should_run_task_at_least_5_times()
        {
            // given
            var timer = new Timer();
            int calls = 0;
            DateTime timeout = DateTime.Now.AddSeconds(4);

            // when
            timer.RunEvery(() => calls++, TimeSpan.FromMilliseconds(1));

            while (calls < 5 && DateTime.Now < timeout)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(5));
            }

            // then
            calls.ShouldBeGreaterThanOrEqualTo(5);
        }

        [Fact]
        public void should_throw_exception_if_a_second_timer_is_created()
        {
            // given
            var timer = new Timer();
            timer.RunEvery(() => { }, TimeSpan.FromMilliseconds(1));

            // when + then
            Assert.Throws<Timer.TimerAlreadyInitialisedException>(() => timer.RunEvery(() => { }, TimeSpan.FromMinutes(1)));
        }
    }
}