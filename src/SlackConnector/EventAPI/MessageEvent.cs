using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class MessageEvent : CommonInboundEvent
    {
		public class MessageReaction
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("count")]
			public int Count { get; set; }

			[JsonProperty("users")]
			public string[] Users { get; set; }
		}

		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("is_starred")]
		public bool? IsStarred { get; set; }

		[JsonProperty("pinned_to")]
		public string[] PinnedTo { get; set; }

		[JsonProperty("reactions")]
		public MessageReaction[] Reactions { get; set; }

		[JsonProperty("thread_ts")]
		public string ThreadTimestamp { get; set; }
	}
}
