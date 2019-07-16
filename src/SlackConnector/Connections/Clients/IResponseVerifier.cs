using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients
{
    public interface IResponseVerifier
    {
        void VerifyResponse(StandardResponse response);

		void VerifyResponse(DialogResponse response);

	}
}