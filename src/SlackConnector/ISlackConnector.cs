using System.Threading.Tasks;
using SlackConnector.Connections;

namespace SlackConnector
{
    public interface ISlackConnector
    {
        Task<ISlackConnection> Connect(string slackKey, ProxySettings proxySettings = null);
    }
}