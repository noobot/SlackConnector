using System.Threading.Tasks;

namespace SlackConnector.Connections.Clients.Channel
{
    internal interface IChannelClient
    {
        Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user);
    }
}