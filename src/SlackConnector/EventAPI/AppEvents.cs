using Newtonsoft.Json;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class AppMentionEvent : InboundEvent
    {
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("channel")]
		public string Channel { get; set; }
	}

	public class AppRateLimitedEvent : InboundOuterEvent
	{
		[JsonProperty("minute_rate_limited")]
		[JsonConverter(typeof(SecondEpochConverter))]
		public DateTime MinuteRateLimited { get; set; }
	}
}
