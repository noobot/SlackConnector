using System.Collections.Generic;
using System.Linq;
using SlackConnector.Connections.Models;
using SlackConnector.Models;

namespace SlackConnector
{
    public static class SlackConnectionExtensions
    {
        public static IEnumerable<SlackChatHub> ConnectedDMs(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.DM);
        }

        public static IEnumerable<SlackChatHub> ConnectedChannels(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Channel);
        }

        public static IEnumerable<SlackChatHub> ConnectedGroups(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Group);
        }

        internal static SlackChatHub ToChatHub(this Channel channel)
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

        internal static SlackChatHub ToChatHub(this Group group)
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

        internal static SlackUser ToSlackUser(this User user)
        {
            var slackUser = new SlackUser()
            {
                Id = user.Id,
                Name = user.Name,
                TimeZoneOffset = user.TimeZoneOffset,
                IsBot = user.IsBot
            };
            if (!string.IsNullOrWhiteSpace(user.Presence))
            {
                slackUser.Online = user.Presence == "active";
            }
            return slackUser;
        }
    }
}