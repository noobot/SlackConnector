using System.Threading.Tasks;

namespace SlackConnector.Connections.Messaging
{
    internal interface IChannelMessenger
    {
        Task JoinDirectMessageChannel(string slackKey, string user);
    }
}