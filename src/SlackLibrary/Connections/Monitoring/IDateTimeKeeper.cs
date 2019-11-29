using System;

namespace SlackLibrary.Connections.Monitoring
{
    internal interface IDateTimeKeeper
    {
        void SetDateTimeToNow();
        bool HasDateTime();
        TimeSpan TimeSinceDateTime();
    }
}