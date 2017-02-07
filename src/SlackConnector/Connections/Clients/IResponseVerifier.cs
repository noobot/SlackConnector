using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients
{
    internal interface IResponseVerifier
    {
        void VerifyResponse(StandardResponse response);
    }
}