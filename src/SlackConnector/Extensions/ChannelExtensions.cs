using SlackConnector.Connections.Models;
using SlackConnector.Models;

namespace SlackConnector.Extensions
{
    internal static class ChannelExtensions
    {
        public static SlackChatHub ToChatHub(this Channel channel)
        {
            var newChannel = new SlackChatHub
            {
                Id = channel.Id,
                Name = "#" + channel.Name,
                Type = SlackChatHubType.Channel,
                Members = channel.Members
            };
            return newChannel;
        }
    }
}