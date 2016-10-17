using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class ChannelsResponse : StandardResponse
    {
         internal Channel[] Channels { get; set; }
    }
}