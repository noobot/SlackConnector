using System.Threading.Tasks;
using SlackLibrary.Models;

namespace SlackLibrary.Tests.Unit.Stubs
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