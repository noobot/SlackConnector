using System;
using System.Threading.Tasks;

namespace SlackLibrary.EventHandlers
{
    public delegate Task PongEventHandler(DateTime timestamp);
}
