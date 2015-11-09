using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Models;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ISlackConnectionFactory _slackConnectionFactory;

        public SlackConnector() : this(new ConnectionFactory(), new SlackConnectionFactory())
        { }

        internal SlackConnector(IConnectionFactory connectionFactory, ISlackConnectionFactory slackConnectionFactory)
        {
            _connectionFactory = connectionFactory;
            _slackConnectionFactory = slackConnectionFactory;
        }

        public async Task<ISlackConnection> Connect(string slackKey)
        {
            var handshakeClient = _connectionFactory.CreateHandshakeClient();
            SlackHandshake handshake = await handshakeClient.FirmShake(slackKey);

            var connectionInfo = new ConnectionInformation
            {
                Self = new ContactDetails { Id = handshake.Self.Id, Name = handshake.Self.Name },
                Team = new ContactDetails { Id = handshake.Team.Id, Name = handshake.Team.Name },
                Users = GenerateUsers(handshake.Users)
            };

            return _slackConnectionFactory.Create(connectionInfo);
        }

        private Dictionary<string, string> GenerateUsers(User[] users)
        {
            return users.ToDictionary(user => user.Id, user => user.Name);
        }
    }
}