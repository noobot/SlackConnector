using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class ChannelsResponse : StandardResponse
    {
         public Channel[] Channels { get; set; }
    }
}