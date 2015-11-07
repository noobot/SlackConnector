using RestSharp;

namespace SlackConnector.Connections
{
    internal interface IRestSharpFactory
    {
        IRestClient CreateClient(string baseUrl);
    }
}