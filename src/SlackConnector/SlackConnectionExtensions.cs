using System;
using System.Collections.Generic;
using System.Linq;
using SlackConnector.Models;

namespace SlackConnector
{
    public static class SlackConnectionExtensions
    {
        /// <summary>
        /// Will try and find a direct messag the bot is connected to with that user name, e.g. simon
        /// </summary>
        public static SlackChatHub ConnectedDM(this ISlackConnection slackConnection, string userName)
        {
            return slackConnection.ConnectedDMs().FirstOrDefault(x => x.Name.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<SlackChatHub> ConnectedDMs(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.DM);
        }

        /// <summary>
        /// Will try and find a channel the bot is connected to with that name, e.g. #general
        /// </summary>
        public static SlackChatHub ConnectedChannel(this ISlackConnection slackConnection, string channelName)
        {
            return slackConnection.ConnectedChannels().FirstOrDefault(x => x.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<SlackChatHub> ConnectedChannels(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Channel);
        }

        /// <summary>
        /// Will try and find a channel the bot is connected to with that name, e.g. #group
        /// </summary>
        public static SlackChatHub ConnectedGroup(this ISlackConnection slackConnection, string groupName)
        {
            return slackConnection.ConnectedGroups().FirstOrDefault(x => x.Name.Equals(groupName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<SlackChatHub> ConnectedGroups(this ISlackConnection slackConnection)
        {
            return slackConnection.ConnectedHubs.Values.Where(hub => hub.Type == SlackChatHubType.Group);
        }
    }
}