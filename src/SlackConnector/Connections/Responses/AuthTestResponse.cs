using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
	public class AuthTestResponse : StandardResponse
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("team")]
		public string Team { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("user_id")]
		public string UserId { get; set; }
	}
}
