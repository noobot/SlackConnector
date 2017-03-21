using System;
using System.Threading.Tasks;

namespace SlackConnector.Connections.Monitoring
{
    internal class PingPongMonitor : IPingPongMonitor
    {
        private readonly ITimer _timer;
        private readonly IDateTimeKeeper _dateTimeKeeper;

        private TimeSpan _pongTimeout;
        private Func<Task> _pingMethod;
        private Func<Task> _reconnectMethod;

        public PingPongMonitor(ITimer timer, IDateTimeKeeper dateTimeKeeper)
        {
            _timer = timer;
            _dateTimeKeeper = dateTimeKeeper;
        }

        public async Task StartMonitor(Func<Task> pingMethod, Func<Task> reconnectMethod, TimeSpan pongTimeout)
        {
            if (_dateTimeKeeper.HasDateTime())
            {
                throw new MonitorAlreadyStartedException();
            }

            _pingMethod = pingMethod;
            _reconnectMethod = reconnectMethod;
            _pongTimeout = pongTimeout;

            _timer.RunEvery(TimerTick, TimeSpan.FromSeconds(5));
            await pingMethod();
        }

        private void TimerTick()
        {
            if (_dateTimeKeeper.HasDateTime() && _dateTimeKeeper.TimeSinceDateTime() > _pongTimeout)
            {
                _reconnectMethod().Wait();
            }

            _pingMethod();
        }

        public void Pong()
        {
            _dateTimeKeeper.SetDateTimeToNow();
        }
    }
}