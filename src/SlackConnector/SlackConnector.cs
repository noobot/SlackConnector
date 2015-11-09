using System.Threading.Tasks;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        private readonly ISlackConnectionFactory _slackConnectionFactory;

        public SlackConnector() : this(new SlackConnectionFactory())
        { }

        internal SlackConnector(ISlackConnectionFactory slackConnectionFactory)
        {
            _slackConnectionFactory = slackConnectionFactory;
        }

        public Task<ISlackConnection> Connect(string slackKey)
        {
            throw new System.NotImplementedException();
        }
    }
}