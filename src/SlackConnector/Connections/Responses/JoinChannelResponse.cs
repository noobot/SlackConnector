using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class JoinChannelResponse : DefaultStandardResponse
	{
        public Channel Channel { get; set; }
    }
}