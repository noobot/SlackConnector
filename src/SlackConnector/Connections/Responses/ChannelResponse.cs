using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class ChannelResponse : StandardResponse
    {
        public Channel Channel { get; set; }
    }
}
