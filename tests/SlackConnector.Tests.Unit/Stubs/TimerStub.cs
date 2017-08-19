using System;
using SlackConnector.Connections.Monitoring;

namespace SlackConnector.Tests.Unit.Stubs
{
    public class TimerStub : ITimer
    {
        public Action RunEvery_Action { get; private set; }
        public TimeSpan RunEvery_Tick { get; private set; }

        public void RunEvery(Action action, TimeSpan tick)
        {
            RunEvery_Action = action;
            RunEvery_Tick = tick;
        }

        public void Dispose()
        {
        }
    }
}