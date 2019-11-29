using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Responses
{
    internal class ChannelResponse : DefaultStandardResponse
	{
        public Channel Channel { get; set; }
    }
}
