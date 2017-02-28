
using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SlackConnector.Connections.Monitoring;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    [TestFixture]
    public class PingPongMonitorTests
    {
        private IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Freeze<Mock<ITimer>>();
        }

        [Test]
        public async Task should_call_ping_when_start_monitor_is_called()
        {
            // given
            var monitor = _fixture.Create<PingPongMonitor>();
            bool pingCalled = false;
            Func<Task> pingMethod = () => { pingCalled = true; return Task.CompletedTask; };

            // when
            await monitor.StartMonitor(pingMethod, null, TimeSpan.MinValue);

            // then
            Assert.That(pingCalled, Is.True);
        }

        [Test]
        public async Task should_start_timer_when_monitor_is_started()
        {
            // given
            var monitor = _fixture.Create<PingPongMonitor>();

            // when
            await monitor.StartMonitor(() => Task.CompletedTask, () => Task.CompletedTask, TimeSpan.MinValue);

            // then
            _fixture
                .Create<Mock<ITimer>>()
                .Verify(x => x.RunEvery(It.IsAny<Action>(), TimeSpan.FromSeconds(5)), Times.Once);
        }
    }
}