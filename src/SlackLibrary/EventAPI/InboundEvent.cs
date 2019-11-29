using Newtonsoft.Json;
using SlackLibrary.Serialising;
using System;

namespace SlackLibrary.EventAPI
{
	public class InboundEvent
	{
		[JsonProperty("type")]
		[JsonConverter(typeof(EventTypeConverter))]
		public EventType Type { get; set; }
	}

	public class CommonInboundEvent : InboundEvent
	{
		[JsonProperty("event_ts")]
		public string EventTimestamp { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }
	}
}