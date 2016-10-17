using System.Threading.Tasks;

namespace SlackConnector.Connections.Clients.Channel
{
    internal interface IChannelClient
    {
        Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user);

        Task<Models.Channel[]> GetChannels(string slackKey);

        Task<Models.Group[]> GetGroups(string slackKey);

        Task<Models.User[]> GetUsers(string slackKey);
    }
}