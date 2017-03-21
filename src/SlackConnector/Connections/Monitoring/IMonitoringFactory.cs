using System;

namespace SlackConnector.Connections.Monitoring
{
    internal interface IMonitoringFactory
    {
        IPingPongMonitor CreatePingPongMonitor();
    }

    internal class MonitoringFactory : IMonitoringFactory
    {
        public IPingPongMonitor CreatePingPongMonitor()
        {
            throw new NotImplementedException();
            //return new PingPongMonitor(new Timer(), new );
        }
    }
}