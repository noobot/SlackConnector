using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SlackConnector.Connections.Monitoring;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    [TestFixture]
    internal class PingPongMonitorTests
    {
        [Test, AutoMoqData]
        public async Task should_call_ping_when_start_monitor_is_called(PingPongMonitor monitor)
        {
            // given
            bool pingCalled = false;
            Func<Task> pingMethod = () => { pingCalled = true; return Task.CompletedTask; };

            // when
            await monitor.StartMonitor(pingMethod, null, TimeSpan.MinValue);

            // then
            Assert.That(pingCalled, Is.True);
        }

        [Test, AutoMoqData]
        public async Task should_start_timer_when_monitor_is_started([Frozen]Mock<ITimer> timerMock, PingPongMonitor monitor)
        {
            // given

            // when
            await monitor.StartMonitor(() => Task.CompletedTask, () => Task.CompletedTask, TimeSpan.MinValue);

            // then
            timerMock
                .Verify(x => x.RunEvery(It.IsAny<Action>(), TimeSpan.FromSeconds(5)), Times.Once);
        }
    }
}