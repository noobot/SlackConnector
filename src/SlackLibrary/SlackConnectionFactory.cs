using System.Threading.Tasks;
using SlackLibrary.BotHelpers;
using SlackLibrary.Connections;
using SlackLibrary.Connections.Monitoring;
using SlackLibrary.Models;

namespace SlackLibrary
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