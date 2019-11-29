using SlackLibrary.Connections.Responses;
using System;
using System.Runtime.Serialization;

namespace SlackLibrary.Exceptions
{
	public class ResponseCommunicationException : CommunicationException
	{
		public ResponseCommunicationException(DefaultStandardResponse response, string message) : base(message)
		{
			Response = response;
		}

		public ResponseCommunicationException(DefaultStandardResponse response, string message, Exception innerException) : base(message, innerException)
		{
			Response = response;
		}

		public DefaultStandardResponse Response { get; }
	}

	public class CommunicationException : Exception
    {
        public CommunicationException(string message) : base(message)
        { }

        public CommunicationException(string message, Exception innerException) : base(message, innerException)
        { }

		public string SlackError { get; internal set; }
	}
}