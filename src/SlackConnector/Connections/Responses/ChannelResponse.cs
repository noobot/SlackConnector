using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class ChannelResponse : DefaultStandardResponse
	{
        public Channel Channel { get; set; }
    }
}
