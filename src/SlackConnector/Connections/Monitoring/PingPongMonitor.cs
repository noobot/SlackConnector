using System;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;

namespace SlackConnector.Connections.Monitoring
{
    internal class PingPongMonitor : IPingPongMonitor
    {
        private readonly Timer _timer;
        private Func<Task> _pingMethod;

        public PingPongMonitor() : this(new Timer())
        { }

        private PingPongMonitor(Timer timer)
        {
            _timer = timer;
        }

        public void StartMonitor(Func<Task> pingMethod, PongEventHandler onPong)
        {
            _pingMethod = pingMethod;
        }
    }
}