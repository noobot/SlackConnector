using System.Collections.Generic;
using System.Linq;
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
    }
}