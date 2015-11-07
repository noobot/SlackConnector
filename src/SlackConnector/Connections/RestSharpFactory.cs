using RestSharp;

namespace SlackConnector.Connections
{
    internal class RestSharpFactory : IRestSharpFactory
    {
        public IRestClient CreateClient(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}