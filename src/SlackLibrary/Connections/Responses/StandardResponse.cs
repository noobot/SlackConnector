using Newtonsoft.Json;
using System.Collections.Generic;

namespace SlackLibrary.Connections.Responses
{
	public class ResponseMetadata
	{
		[JsonProperty("messages")]
		public IEnumerable<string> Messages { get; set; }
	}

	public class DefaultStandardResponse : StandardResponse<ResponseMetadata>
	{
	}

	public class StandardResponse<T>
    {
        public bool Ok { get; set; }
        public string Error { get; set; }

		[JsonProperty("response_metadata")]
		public T ResponseMetadata { get; set; }
	}
}