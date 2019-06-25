using Newtonsoft.Json;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class ChannelArchiveEvent : InboundEvent
    {
		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }
	}

	public class ChannelCreatedEvent : InboundEvent
	{
		public class ChannelCreated
		{
			[JsonProperty("id")]
			public string Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("created")]
			[JsonConverter(typeof(SecondEpochConverter))]
			public DateTime Created { get; set; }

			[JsonProperty("creator")]
			public string Creator { get; set; }
		}

		[JsonProperty("channel")]
		public ChannelCreated Channel { get; set; }
	}

	public class ChannelDeletedEvent : InboundEvent
	{
		[JsonProperty("channel")]
		public string Channel { get; set; }
	}

	public class ChannelHistoryChangedEvent : InboundEvent
	{
		[JsonProperty("latest")]
		public string Latest { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("event_ts")]
		public string EventTimestamp { get; set; }
	}

	public class ChannelRenameEvent : InboundEvent
	{
		public class ChannelRenamed
		{
			[JsonProperty("id")]
			public string Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("created")]
			[JsonConverter(typeof(SecondEpochConverter))]
			public DateTime Created { get; set; }
		}

		[JsonProperty("channel")]
		public ChannelRenamed Channel { get; set; }
	}
}
