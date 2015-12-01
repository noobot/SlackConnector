using RestSharp;

namespace SlackConnector.Connections.Messaging
{
    internal interface IResponseVerifier
    {
        T VerifyResponse<T>(IRestResponse response) where T : class;
    }
}