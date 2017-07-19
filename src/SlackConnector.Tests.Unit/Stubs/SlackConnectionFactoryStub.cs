using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class SlackConnectionFactoryStub : ISlackConnectionFactory
    {
        public ConnectionInformation Create_ConnectionInformation { get; private set; }
        public SlackConnectionStub Create_Value { get; set; }

        public Task<ISlackConnection> Create(ConnectionInformation connectionInformation)
        {
            Create_ConnectionInformation = connectionInformation;
            return Task.FromResult<ISlackConnection>(Create_Value);
        }
    }
}