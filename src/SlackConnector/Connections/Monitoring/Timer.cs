using System;

namespace SlackConnector.Connections.Monitoring
{
    internal class Timer : ITimer
    {
        private System.Threading.Timer _timer;

        public void RunEvery(Action action, TimeSpan timeSpan)
        {
            if (_timer != null)
            {
                throw new TimerAlreadyInitialisedException();
            }

            _timer = new System.Threading.Timer(state => action(), null, TimeSpan.Zero, timeSpan);
        }

        public class TimerAlreadyInitialisedException : Exception
        {
            
        }
    }
}