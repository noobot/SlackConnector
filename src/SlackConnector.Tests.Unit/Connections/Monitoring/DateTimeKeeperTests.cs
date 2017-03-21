using NUnit.Framework;
using SlackConnector.Connections.Monitoring;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    internal class DateTimeKeeperTests
    {
        [Test, AutoMoqData]
        public void should_throw_exception_if_get_time_is_called_before_being_set(DateTimeKeeper dateTimeKeeper)
        {
            Assert.Throws<DateTimeKeeper.DateTimeNotSetException>(() => dateTimeKeeper.TimeSinceDateTime());
            Assert.That(dateTimeKeeper.HasDateTime(), Is.False);
        }
    }
}