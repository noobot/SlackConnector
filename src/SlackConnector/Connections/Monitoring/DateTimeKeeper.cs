using System;

namespace SlackConnector.Connections.Monitoring
{
    internal class DateTimeKeeper : IDateTimeKeeper
    {
        public void SetDateTimeToNow()
        {
            throw new NotImplementedException();
        }

        public bool HasDateTime()
        {
            return false;
        }

        public TimeSpan TimeSinceDateTime()
        {
            throw new DateTimeNotSetException();
        }

        public class DateTimeNotSetException : Exception
        { }
    }
}