using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using AutoFixture.Xunit2;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Tests.Unit.Stubs;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    public class PingPongMonitorTests
    {
        [Theory, AutoMoqData]
        private async Task should_call_ping_when_start_monitor_is_called(PingPongMonitor monitor)
        {
            // given
            bool pingCalled = false;
            Func<Task> pingMethod = () => { pingCalled = true; return Task.CompletedTask; };

            // when
            await monitor.StartMonitor(pingMethod, null, TimeSpan.MinValue);

            // then
            pingCalled.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        private async Task should_start_timer_when_monitor_is_started([Frozen]Mock<ITimer> timerMock, PingPongMonitor monitor)
        {
            // given

            // when
            await monitor.StartMonitor(() => Task.CompletedTask, () => Task.CompletedTask, TimeSpan.MinValue);

            // then
            timerMock
                .Verify(x => x.RunEvery(It.IsAny<Action>(), TimeSpan.FromSeconds(5)), Times.Once);
        }

        [Theory, AutoMoqData]
        private async Task should_call_ping_when_timer_ticks(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeper)
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
            pingCalls.ShouldBe(2);
            reconnectCalled.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private async Task should_throw_exception_if_start_monitor_is_called_twice([Frozen]Mock<IDateTimeKeeper> dateTimeKeeper, PingPongMonitor monitor)
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
            Assert.IsType<MonitorAlreadyStartedException>(exception.InnerExceptions[0]);
        }

        [Theory, AutoMoqData]
        private async Task should_initiate_reconnect_if_timerkeeper_is_beyond_timeout(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeperMock)
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
            reconnectCalled.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        private async Task should_not_initiate_reconnect_if_reconnection_is_already_underway(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeperMock)
        {
            // given
            var monitor = new PingPongMonitor(timerStub, dateTimeKeeperMock.Object);

            dateTimeKeeperMock
                .Setup(x => x.TimeSinceDateTime())
                .Returns(TimeSpan.FromMinutes(2));

            int reconnectCalls = 0;
            Func<Task> reconnect = () =>
            {
                reconnectCalls++;
                if (reconnectCalls == 1)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                return Task.CompletedTask;
            };
            await monitor.StartMonitor(() => Task.CompletedTask, reconnect, TimeSpan.FromMinutes(1));

            dateTimeKeeperMock
                .Setup(x => x.HasDateTime())
                .Returns(true);

            // when
            var thing = Task.Factory.StartNew(() => timerStub.RunEvery_Action());
            Thread.Sleep(TimeSpan.FromSeconds(1));
            timerStub.RunEvery_Action();
            
            // then
            reconnectCalls.ShouldBe(1);
        }

        [Theory, AutoMoqData]
        private async Task should_not_initiate_reconnect_if_timerkeeper_hasnt_been_set(TimerStub timerStub, Mock<IDateTimeKeeper> dateTimeKeeperMock)
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
            reconnectCalled.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private void should_update_time_keeper_when_pong_is_received([Frozen]Mock<IDateTimeKeeper> dateTimeKeeperMock, PingPongMonitor monitor)
        {
            // given

            // when
            monitor.Pong();

            // then
            dateTimeKeeperMock.Verify(x => x.SetDateTimeToNow(), Times.Once);
        }
    }
}