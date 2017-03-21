using System;

namespace SlackConnector.Connections.Monitoring
{
    internal interface IDateTimeKeeper
    {
        void SetDateTimeToNow();
        bool HasDateTime();
        TimeSpan TimeSinceDateTime();
    }
}