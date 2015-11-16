using System.Threading.Tasks;

namespace SlackConnector
{
    public interface ISlackConnector
    {
        Task<ISlackConnection> Connect(string slackKey);
    }
}