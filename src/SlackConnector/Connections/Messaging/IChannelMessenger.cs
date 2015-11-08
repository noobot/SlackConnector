using System.Threading.Tasks;
using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Messaging
{
    internal interface IChannelMessenger
    {
        Task<Channel> JoinDirectMessageChannel(string slackKey, string user);
    }
}