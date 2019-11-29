using System.Threading.Tasks;

namespace SlackLibrary
{
    public interface ISlackConnector
    {
        Task<ISlackConnection> Connect(string slackKey);
    }
}