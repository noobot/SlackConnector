using SlackConnector.Models;

namespace SlackConnector
{
    internal interface ISlackConnectionFactory
    {
        ISlackConnection Create(ConnectionInformation connectionInformation);
    }
}