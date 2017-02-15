using System;
using System.Threading.Tasks;

namespace SlackConnector.EventHandlers
{
    public delegate Task PongEventHandler(DateTime timestamp);
}
