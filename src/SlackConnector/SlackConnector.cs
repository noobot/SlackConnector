using System.Threading.Tasks;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        public Task<ISlackConnection> Connect(string slackKey)
        {
            throw new System.NotImplementedException();
        }
    }
}