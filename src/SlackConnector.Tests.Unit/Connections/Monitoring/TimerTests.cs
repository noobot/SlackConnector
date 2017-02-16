using System;
using System.Threading;
using NUnit.Framework;
using Timer = SlackConnector.Connections.Monitoring.Timer;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    [TestFixture]
    public class TimerTests
    {
        [Test]
        public void should_run_task_at_least_5_times()
        {
            // given
            var timer = new Timer();
            int calls = 0;
            DateTime timeout = DateTime.Now.AddSeconds(2);

            // when
            timer.RunEvery(() => calls++, TimeSpan.FromMilliseconds(1));

            while (calls < 5 && DateTime.Now < timeout)
            {
                Thread.Sleep(5);
            }

            // then
            Assert.That(calls, Is.AtLeast(5));
        }
    }
}