using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;

namespace SlackConnector.Connections.Clients
{
    internal class ResponseVerifier : IResponseVerifier
    {
        public void VerifyResponse(StandardResponse response)
        {
            if (!response.Ok)
            {
                throw new CommunicationException($"Error occured while posting message '{response.Error}'");
            }
        }
    }
}