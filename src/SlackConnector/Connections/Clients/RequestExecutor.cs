using System.Threading.Tasks;
using RestSharp;

namespace SlackConnector.Connections.Clients
{
    internal class RequestExecutor : IRequestExecutor
    {
        internal const string SLACK_URL = "https://slack.com";

        private readonly IRestSharpFactory _restSharpFactory;
        private readonly IResponseVerifier _responseVerifier;

        public RequestExecutor(IRestSharpFactory restSharpFactory, IResponseVerifier responseVerifier)
        {
            _restSharpFactory = restSharpFactory;
            _responseVerifier = responseVerifier;
        }

        public async Task<T> Execute<T>(IRestRequest request) where T : class
        {
            IRestClient client = _restSharpFactory.CreateClient(SLACK_URL);
            IRestResponse response = await client.ExecutePostTaskAsync(request);
            return _responseVerifier.VerifyResponse<T>(response);
        }
    }
}