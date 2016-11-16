using SlackConnector.Connections.Models;
using SlackConnector.Models;

namespace SlackConnector.Extensions
{
    internal static class GroupExtensions
    {
        public static SlackChatHub ToChatHub(this Group group)
        {
            var newGroup = new SlackChatHub
            {
                Id = group.Id,
                Name = "#" + group.Name,
                Type = SlackChatHubType.Group,
                Members = group.Members
            };

            return newGroup;
        }
    }
}