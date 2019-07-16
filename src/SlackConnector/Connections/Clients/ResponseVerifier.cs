using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;

namespace SlackConnector.Connections.Clients
{
    public class ResponseVerifier : IResponseVerifier
    {
        public void VerifyResponse(StandardResponse response)
        {
            if (!response.Ok)
            {
                throw new CommunicationException($"Error occured while posting message '{response.Error}'") { SlackError = response.Error };
            }
        }

		public void VerifyResponse(DialogResponse response)
		{
			if (!response.Ok)
			{
				if (response.Error == "validation_errors")
				{
					var validationException = new DialogValidationException(response);
					throw new CommunicationException($"Error occured while posting dialog '{response.Error}'", validationException) { SlackError = response.Error };
				}
				else
				{
					throw new CommunicationException($"Error occured while posting dialog '{response.Error}'") { SlackError = response.Error };
				}
			}
		}
	}
}