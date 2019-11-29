namespace SlackLibrary.Connections.Monitoring
{
    internal interface IMonitoringFactory
    {
        IPingPongMonitor CreatePingPongMonitor();
    }
}