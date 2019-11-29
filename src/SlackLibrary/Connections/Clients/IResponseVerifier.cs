using SlackLibrary.Connections.Responses;

namespace SlackLibrary.Connections.Clients
{
    public interface IResponseVerifier
    {
		void VerifyResponse(DefaultStandardResponse response);

		void VerifyResponse<T>(StandardResponse<T> response);

		void VerifyDialogResponse(DefaultStandardResponse response);

	}
}