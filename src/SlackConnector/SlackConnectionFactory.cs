using System.Threading.Tasks;
using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Connections.Monitoring;
using SlackConnector.Models;

namespace SlackConnector
{
    internal class SlackConnectionFactory : ISlackConnectionFactory
    {
        public async Task<ISlackConnection> Create(ConnectionInformation connectionInformation)
        {
            var slackConnection = new SlackConnection(new ConnectionFactory(), new MentionDetector(), new MonitoringFactory());
            await slackConnection.Initialise(connectionInformation);
            return slackConnection;
        }
    }
}