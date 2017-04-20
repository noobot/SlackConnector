using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class JoinChannelResponse : StandardResponse
    {
        public new Channel Channel { get; set; }
    }
}