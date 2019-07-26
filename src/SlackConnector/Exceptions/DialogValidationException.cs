using SlackConnector.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Exceptions
{

	[Serializable]
	public class DialogValidationException : Exception
	{
		public DialogValidationException(DefaultStandardResponse dialogResponse) {
			DialogResponse = dialogResponse;
		}
		public DialogValidationException(string message) : base(message) { }
		public DialogValidationException(string message, Exception inner) : base(message, inner) { }
		protected DialogValidationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

		public DefaultStandardResponse DialogResponse { get; }
	}
}
