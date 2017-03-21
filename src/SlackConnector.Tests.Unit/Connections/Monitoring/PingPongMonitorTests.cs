using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Tests.Unit.Stubs;

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

        [Test, AutoMoqData]
        public async Task should_call_ping_when_timer_ticks(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeper)
        {
            // given
            var monitor = new PingPongMonitor(timerStub, dateTimeKeeper.Object);

            dateTimeKeeper
                .Setup(x => x.TimeSinceDateTime())
                .Returns(TimeSpan.FromSeconds(1));

            int pingCalls = 0;
            Func<Task> pingMethod = () => { pingCalls++; return Task.CompletedTask; };

            bool reconnectCalled = false;
            Func<Task> reconnectMethod = () => { reconnectCalled = true; return Task.CompletedTask; };

            await monitor.StartMonitor(pingMethod, reconnectMethod, TimeSpan.FromMinutes(1));

            // when
            timerStub.RunEvery_Action();

            // then
            Assert.That(pingCalls, Is.EqualTo(2));
            Assert.That(reconnectCalled, Is.False);
        }

        [Test, AutoMoqData]
        public async Task should_throw_exception_if_start_monitor_is_called_twice([Frozen]Mock<IDateTimeKeeper> dateTimeKeeper, PingPongMonitor monitor)
        {
            // given
            await monitor.StartMonitor(() => Task.CompletedTask, () => Task.CompletedTask, TimeSpan.MinValue);
            dateTimeKeeper
                .Setup(x => x.HasDateTime())
                .Returns(true);

            // when
            var exception = Assert.Throws<AggregateException>(() =>
                                monitor.StartMonitor(() => Task.CompletedTask, () => Task.CompletedTask, TimeSpan.MinValue).Wait());

            // then
            Assert.That(exception.InnerExceptions[0], Is.TypeOf<MonitorAlreadyStartedException>());
        }

        [Test, AutoMoqData]
        public async Task should_initiate_reconnect_if_timerkeeper_is_beyond_timeout(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeperMock)
        {
            // given
            var monitor = new PingPongMonitor(timerStub, dateTimeKeeperMock.Object);
            
            dateTimeKeeperMock
                .Setup(x => x.TimeSinceDateTime())
                .Returns(TimeSpan.FromMinutes(2));

            bool reconnectCalled = false;
            Func<Task> reconnect = () => { reconnectCalled = true; return Task.CompletedTask; };
            await monitor.StartMonitor(() => Task.CompletedTask, reconnect, TimeSpan.FromMinutes(1));

            dateTimeKeeperMock
                .Setup(x => x.HasDateTime())
                .Returns(true);

            // when
            timerStub.RunEvery_Action();

            // then
            Assert.That(reconnectCalled, Is.True);
        }

        [Test, AutoMoqData]
        public async Task should_not_initiate_reconnect_if_timerkeeper_hasnt_been_set(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeperMock)
        {
            // given
            var monitor = new PingPongMonitor(timerStub, dateTimeKeeperMock.Object);

            dateTimeKeeperMock
                .Setup(x => x.HasDateTime())
                .Returns(false);

            dateTimeKeeperMock
                .Setup(x => x.TimeSinceDateTime())
                .Returns(TimeSpan.FromMinutes(2));

            bool reconnectCalled = false;
            Func<Task> reconnect = () => { reconnectCalled = true; return Task.CompletedTask; };
            await monitor.StartMonitor(() => Task.CompletedTask, reconnect, TimeSpan.FromMinutes(1));

            // when
            timerStub.RunEvery_Action();

            // then
            Assert.That(reconnectCalled, Is.False);
        }

        [Test, AutoMoqData]
        public void should_update_time_keeper_when_pong_is_received([Frozen]Mock<IDateTimeKeeper> dateTimeKeeperMock, PingPongMonitor monitor)
        {
            // given

            // when
            monitor.Pong();

            // then
            dateTimeKeeperMock.Verify(x => x.SetDateTimeToNow(), Times.Once);
        }
    }
}