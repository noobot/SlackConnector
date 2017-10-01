using System;
using System.Threading;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Timer = SlackConnector.Connections.Monitoring.Timer;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    public class TimerTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly Timer _timer = new Timer();

        public TimerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void should_run_task_at_least_5_times()
        {
            // given
            var eventTriggered = new AutoResetEvent(false);
            int count = 0;

            // when
            _timer.RunEvery(() =>
            {
               _output.WriteLine("Trigger");
               eventTriggered.Set();
               count++;
            }, TimeSpan.FromMilliseconds(1));
            
            // then
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            eventTriggered.WaitOne(TimeSpan.FromSeconds(1));
            count.ShouldBeGreaterThanOrEqualTo(5);
        }

        [Fact]
        public void should_throw_exception_if_a_second_timer_is_created()
        {
            // given
            _timer.RunEvery(() => { }, TimeSpan.FromMilliseconds(1));

            // when + then
            Assert.Throws<Timer.TimerAlreadyInitialisedException>(() => _timer.RunEvery(() => { }, TimeSpan.FromMinutes(1)));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}