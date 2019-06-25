using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
	public class DialogResponseMetadata
	{
		[JsonProperty("messages")]
		public IEnumerable<string> Messages { get; set; }
	}

	public class DialogResponse : StandardResponse
	{
		[JsonProperty("response_metadata")]
		public DialogResponseMetadata ResponseMetadata { get; set; }
	}
}
