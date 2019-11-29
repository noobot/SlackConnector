using System.Threading.Tasks;
using SlackLibrary.Models;

namespace SlackLibrary
{
    internal interface ISlackConnectionFactory
    {
        Task<ISlackConnection> Create(ConnectionInformation connectionInformation);
    }
}