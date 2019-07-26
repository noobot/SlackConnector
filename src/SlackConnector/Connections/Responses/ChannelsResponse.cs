using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class ChannelsResponse : DefaultStandardResponse
	{
         public Channel[] Channels { get; set; }
    }
}