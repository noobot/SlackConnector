using SlackLibrary.Connections.Responses;
using SlackLibrary.Exceptions;

namespace SlackLibrary.Connections.Clients
{
    public class ResponseVerifier : IResponseVerifier
    {
        public void VerifyResponse(DefaultStandardResponse response)
        {
            if (!response.Ok)
            {
                throw new ResponseCommunicationException(response, $"Error occured while posting message '{response.Error}'") { SlackError = response.Error };
            }
        }

		public void VerifyResponse<T>(StandardResponse<T> response)
		{
			if (!response.Ok)
			{
				throw new CommunicationException($"Error occured while posting message '{response.Error}'") { SlackError = response.Error };
			}
		}

		public void VerifyDialogResponse(DefaultStandardResponse response)
		{
			if (!response.Ok)
			{
				if (response.Error == "validation_errors")
				{
					var validationException = new DialogValidationException(response);
					throw new ResponseCommunicationException(response, $"Error occured while posting dialog '{response.Error}'", validationException) { SlackError = response.Error };
				}
				else
				{
					throw new ResponseCommunicationException(response, $"Error occured while posting dialog '{response.Error}'") { SlackError = response.Error };
				}
			}
		}
	}
}