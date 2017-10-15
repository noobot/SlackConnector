using System.Threading.Tasks;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Channel
{
    internal interface IChannelClient
    {
        Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user);

        Task<Models.Channel> CreateChannel(string slackKey, string channelName);

        Task<Models.Channel> JoinChannel(string slackKey, string channelName);

        Task ArchiveChannel(string slackKey, string channelName);

        Task<string> SetPurpose(string slackKey, string channelName, string purpose);

        Task<string> SetTopic(string slackKey, string channelName, string topic);

        Task<Models.Channel[]> GetChannels(string slackKey);

        Task<Models.Group[]> GetGroups(string slackKey);

        Task<Models.User[]> GetUsers(string slackKey);
    }
}