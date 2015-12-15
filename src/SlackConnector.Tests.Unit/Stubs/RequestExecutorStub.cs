using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class RequestExecutorStub : IRequestExecutor
    {
        public IRestRequest Execute_Request { get; private set; }
        public StandardResponse Execute_Value { get; set; }

        public Task<T> Execute<T>(IRestRequest request) where T : class
        {
            Execute_Request = request;
            return Task.FromResult(Execute_Value as T);
        }
    }
}