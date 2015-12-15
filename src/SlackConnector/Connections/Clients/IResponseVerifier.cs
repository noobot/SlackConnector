using RestSharp;

namespace SlackConnector.Connections.Clients
{
    internal interface IResponseVerifier
    {
        T VerifyResponse<T>(IRestResponse response) where T : class;
    }
}