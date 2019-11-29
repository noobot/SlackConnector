using Newtonsoft.Json;
using SlackLibrary.Serialising;
using System;

namespace SlackLibrary.EventAPI
{
	public enum OuterEventType
	{
		Unknown,
		event_callback,
		app_rate_limited,
		url_verification,
	}

	public class InboundOuterEvent
	{
		[JsonProperty("type")]
		[JsonConverter(typeof(EventTypeConverter))]
		public OuterEventType Type { get; set; }

		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("api_app_id")]
		public string ApiAppId { get; set; }

		public string RawData { get; set; }
	}

	public class InboundOuterCommonEvent : InboundOuterEvent
	{
		[JsonProperty("event")]
		public InboundEvent Event { get; set; }

		[JsonProperty("authed_users")]
		public string[] AuthedUsers { get; set; }

		[JsonProperty("event_id")]
		public string EventId { get; set; }

		[JsonProperty("event_time")]
		[JsonConverter(typeof(SecondEpochConverter))]
		public DateTime EventTime { get; set; }

		public T GetEvent<T>() where T : InboundEvent
		{
			return this.Event as T;
		}
	}
}