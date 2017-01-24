using System.Threading.Tasks;
using RestSharp;

namespace SlackConnector.Connections.Clients
{
    internal interface IRestSharpRequestExecutor
    {
        Task<T> Execute<T>(IRestRequest request) where T : class;
    }
}