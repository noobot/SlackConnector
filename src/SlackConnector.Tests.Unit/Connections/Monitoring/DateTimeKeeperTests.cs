using System;
using System.Threading;
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

        [Test, AutoMoqData]
        public void should_return_time_since_if_datetime_has_been_set(DateTimeKeeper dateTimeKeeper)
        {
            // given

            // when
            dateTimeKeeper.SetDateTimeToNow();

            // then
            Assert.That(dateTimeKeeper.HasDateTime(), Is.True);

            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            Assert.That(dateTimeKeeper.TimeSinceDateTime(), Is.GreaterThan(TimeSpan.FromMilliseconds(100)));
            Assert.That(dateTimeKeeper.TimeSinceDateTime(), Is.LessThan(TimeSpan.FromMilliseconds(600)));
        }
    }
}