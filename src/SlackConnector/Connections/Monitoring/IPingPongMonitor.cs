using System;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;

namespace SlackConnector.Connections.Monitoring
{
    internal interface IPingPongMonitor
    {
        void StartMonitor(Func<Task> pingMethod, PongEventHandler onPong);
    }
}