using Newtonsoft.Json;
using SlackConnector.EventAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
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
