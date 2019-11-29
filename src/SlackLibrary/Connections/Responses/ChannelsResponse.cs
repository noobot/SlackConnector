using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Responses
{
    internal class ChannelsResponse : DefaultStandardResponse
	{
         public Channel[] Channels { get; set; }
    }
}