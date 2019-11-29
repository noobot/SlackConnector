using SlackLibrary.Connections.Models;
using SlackLibrary.Models;

namespace SlackLibrary.Extensions
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