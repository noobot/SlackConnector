using System;
using System.Threading;
using SlackConnector.Connections.Monitoring;
using Xunit;
using XunitShouldExtension;

namespace SlackConnector.Tests.Unit.Connections.Monitoring
{
    internal class DateTimeKeeperTests
    {
        [Theory, AutoMoqData]
        public void should_throw_exception_if_get_time_is_called_before_being_set(DateTimeKeeper dateTimeKeeper)
        {
            Assert.Throws<DateTimeKeeper.DateTimeNotSetException>(() => dateTimeKeeper.TimeSinceDateTime());
            dateTimeKeeper.HasDateTime().ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        public void should_return_time_since_if_datetime_has_been_set(DateTimeKeeper dateTimeKeeper)
        {
            // given

            // when
            dateTimeKeeper.SetDateTimeToNow();

            // then
            dateTimeKeeper.HasDateTime().ShouldBeTrue();

            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            dateTimeKeeper.TimeSinceDateTime().ShouldBeGreaterThan(TimeSpan.FromMilliseconds(100));
            dateTimeKeeper.TimeSinceDateTime().ShouldBeLessThan(TimeSpan.FromMilliseconds(600));
        }
    }
}