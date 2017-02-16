using System;

namespace SlackConnector.Connections.Monitoring
{
    internal class Timer : ITimer
    {
        private System.Threading.Timer _timer;

        public void RunEvery(Action action, TimeSpan timeSpan)
        {
            _timer = new System.Threading.Timer(state => action(), null, TimeSpan.Zero, timeSpan);
        }
    }
}