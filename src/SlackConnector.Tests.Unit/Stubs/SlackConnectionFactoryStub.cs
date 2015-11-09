using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class SlackConnectionFactoryStub : ISlackConnectionFactory
    {
        public ConnectionInformation Create_ConnectionInformation { get; private set; }
        public SlackConnectionStub Create_Value { get; set; }

        public ISlackConnection Create(ConnectionInformation connectionInformation)
        {
            Create_ConnectionInformation = connectionInformation;
            return Create_Value;
        }
    }
}