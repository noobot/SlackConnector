using System;

namespace SlackConnector.Connections.Monitoring
{
    internal class DateTimeKeeper : IDateTimeKeeper
    {
        private DateTime? _dateTime;

        public void SetDateTimeToNow()
        {
            _dateTime = DateTime.Now;
        }

        public bool HasDateTime()
        {
            return _dateTime.HasValue;
        }

        public TimeSpan TimeSinceDateTime()
        {
            if (!_dateTime.HasValue)
            {
                throw new DateTimeNotSetException();
            }

            return DateTime.Now - _dateTime.Value;
        }

        public class DateTimeNotSetException : Exception
        { }
    }
}