using System.Threading.Tasks;
using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Clients
{
    internal interface IChannelClient
    {
        Task<Channel> JoinDirectMessageChannel(string slackKey, string user);
    }
}