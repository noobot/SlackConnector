using System;

namespace SlackConnector.Connections.Monitoring
{
    internal interface ITimer
    {
        void RunEvery(Action action, TimeSpan tick);
    }
}