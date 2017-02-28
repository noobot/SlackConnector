using System;
using System.Threading.Tasks;

namespace SlackConnector.Connections.Monitoring
{
    internal class PingPongMonitor : IPingPongMonitor
    {
        private readonly ITimer _timer;
        private TimeSpan _pongTimeout;
        private Func<Task> _pingMethod;
        private Func<Task> _reconnectMethod;

        private DateTime _lastPong;

        public PingPongMonitor(ITimer timer)
        {
            _timer = timer;
        }

        public async Task StartMonitor(Func<Task> pingMethod, Func<Task> reconnectMethod, TimeSpan pongTimeout)
        {
            _pingMethod = pingMethod;
            _reconnectMethod = reconnectMethod;
            _pongTimeout = pongTimeout;
            _lastPong = DateTime.Now;

            _timer.RunEvery(() => {}, TimeSpan.FromSeconds(5));
            await pingMethod();
        }

        public void Pong(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}