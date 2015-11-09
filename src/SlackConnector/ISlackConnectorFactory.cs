using System.Threading.Tasks;

namespace SlackConnector
{
    public interface ISlackConnectorFactory
    {
        Task<ISlackConnection> Connect(string slackKey);
    }
}