using System.Threading.Tasks;

namespace SlackConnector
{
    public class SlackConnectorFactory : ISlackConnectorFactory
    {
        public Task<ISlackConnection> Connect(string slackKey)
        {
            throw new System.NotImplementedException();
        }
    }
}