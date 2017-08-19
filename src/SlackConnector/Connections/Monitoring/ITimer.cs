using System;

namespace SlackConnector.Connections.Monitoring
{
    internal interface ITimer : IDisposable
    {
        void RunEvery(Action action, TimeSpan tick);
    }
}