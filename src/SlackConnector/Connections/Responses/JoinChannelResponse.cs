using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class JoinChannelResponse : StandardResponse
    {
        public Channel Channel { get; set; }
    }
}