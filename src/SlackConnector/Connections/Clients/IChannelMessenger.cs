using System.Threading.Tasks;
using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Clients
{
    internal interface IChannelMessenger
    {
        Task<Channel> JoinDirectMessageChannel(string slackKey, string user);
    }
}