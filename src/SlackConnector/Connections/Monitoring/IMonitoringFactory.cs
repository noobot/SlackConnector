namespace SlackConnector.Connections.Monitoring
{
    internal interface IMonitoringFactory
    {
        IPingPongMonitor CreatePingPongMonitor();
    }
}