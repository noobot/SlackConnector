using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Responses
{
    internal class JoinChannelResponse : DefaultStandardResponse
	{
        public Channel Channel { get; set; }
    }
}