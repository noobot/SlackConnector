using Newtonsoft.Json;

namespace SlackConnector.EventAPI
{
	public class InboundOuterEvent
	{
		[JsonProperty("token")]
		public string Token { get; set; }


		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("api_app_id")]
		public string ApiAppId { get; set; }

		[JsonProperty("event")]
		public InboundEvent Event { get; set; }

		[JsonProperty("authed_users")]
		public string[] AuthedUsers { get; set; }

		[JsonProperty("event_id")]
		public string EventId { get; set; }

		[JsonProperty("event_time")]
		public int EventTime { get; set; }
	}
}