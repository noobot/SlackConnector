using System;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;

namespace SlackConnector.Connections.Monitoring
{
    internal class PingPongMonitor : IPingPongMonitor
    {
        private readonly ITimer _timer;
        private readonly TimeSpan _maximumTimeBetweenPongs = TimeSpan.FromMinutes(1);
        private Func<Task> _pingMethod;
        private DateTime _lastPong;

        public PingPongMonitor() : this(new Timer())
        { }

        public PingPongMonitor(ITimer timer)
        {
            _timer = timer;
        }

        public void StartMonitor(Func<Task> pingMethod)
        {
            _lastPong = DateTime.Now;
            _pingMethod = pingMethod;
        }

        public void Pong(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}