using SlackConnector.Models;

namespace SlackConnector.EventHandlers
{
    public delegate void MessageReceivedEventHandler(ResponseContext message);
}