using Newtonsoft.Json;
using SlackLibrary.EventAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Responses
{
    public class MessageResponse : DefaultStandardResponse
	{
		public string Channel { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("message")]
		public MessageEvent Message { get; set; }
	}
}
