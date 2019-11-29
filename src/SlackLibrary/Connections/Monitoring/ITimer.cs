using System;

namespace SlackLibrary.Connections.Monitoring
{
    internal interface ITimer : IDisposable
    {
        void RunEvery(Action action, TimeSpan tick);
    }
}