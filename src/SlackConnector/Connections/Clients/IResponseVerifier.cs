using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients
{
    internal interface IResponseVerifier
    {
        T VerifyResponse<T>(IRestResponse response) where T : class;
        void VerifyResponse(StandardResponse response);
    }
}