using Newtonsoft.Json;

namespace SlackConnector.EventAPI
{
	public class InboundEvent
	{
		[JsonProperty("type")]
		[JsonConverter(typeof(EventTypeConverter))]
		public EventType Type { get; set; }

		[JsonProperty("event_ts")]
		public string EventTimestamp { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("item")]
		public string Item { get; set; }
	}
}