using RestSharp;

namespace SlackConnector.Connections
{
    public interface IRestSharpFactory
    {
        IRestClient CreateClient(string baseUrl);
    }
}