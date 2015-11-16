using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Models;

namespace SlackConnector
{
    internal class SlackConnectionFactory : ISlackConnectionFactory
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IChatHubInterpreter _chatHubInterpreter;
        private readonly IMentionDetector _mentionDetector;

        public SlackConnectionFactory()
            : this(new ConnectionFactory(), new ChatHubInterpreter(), new MentionDetector())
        { }

        public SlackConnectionFactory(IConnectionFactory connectionFactory, IChatHubInterpreter chatHubInterpreter, IMentionDetector mentionDetector)
        {
            _connectionFactory = connectionFactory;
            _chatHubInterpreter = chatHubInterpreter;
            _mentionDetector = mentionDetector;
        }

        public ISlackConnection Create(ConnectionInformation connectionInformation)
        {
            var slackConnection = new SlackConnection(_connectionFactory, _chatHubInterpreter, _mentionDetector);
            slackConnection.Initialise(connectionInformation);
            return slackConnection;
        }
    }
}