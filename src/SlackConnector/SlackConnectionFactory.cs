using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Models;

namespace SlackConnector
{
    internal class SlackConnectionFactory : ISlackConnectionFactory
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IMentionDetector _mentionDetector;

        public SlackConnectionFactory()
            : this(new ConnectionFactory(), new MentionDetector())
        { }

        public SlackConnectionFactory(IConnectionFactory connectionFactory, IMentionDetector mentionDetector)
        {
            _connectionFactory = connectionFactory;
            _mentionDetector = mentionDetector;
        }

        public ISlackConnection Create(ConnectionInformation connectionInformation)
        {
            var slackConnection = new SlackConnection(_connectionFactory, _mentionDetector);
            slackConnection.Initialise(connectionInformation);
            return slackConnection;
        }
    }
}