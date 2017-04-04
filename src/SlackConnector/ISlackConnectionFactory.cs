using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector
{
    internal interface ISlackConnectionFactory
    {
        Task<ISlackConnection> Create(ConnectionInformation connectionInformation);
    }
}