using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
	public class MessageCommonEvent : CommonInboundEvent
	{
		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("subtype")]
		[JsonConverter(typeof(EnumConverter))]
		public MessageSubType SubType { get; set; }

	}

	public class MessageChangedEvent : MessageCommonEvent
	{
		public MessageEvent Message { get; set; }

		[JsonProperty("previous_message")]
		public MessageEvent PreviousMessage { get; set; }
	}

	public class MessageDeletedEvent : MessageCommonEvent
	{
		[JsonProperty("deleted_ts")]
		public string DeletedTimestamp { get; set; }

		[JsonProperty("previous_message")]
		public MessageEvent PreviousMessage { get; set; }
	}


	public class MessageEvent : MessageCommonEvent
	{
		public class MessageEdited
		{
			[JsonProperty("user")]
			public string User { get; set; }

			[JsonProperty("ts")]
			public string Timestamp { get; set; }
		}

		public class MessageReaction
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("count")]
			public int Count { get; set; }

			[JsonProperty("users")]
			public string[] Users { get; set; }
		}

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

		[JsonProperty("edited")]
		public MessageEdited Edited { get; set; }

		[JsonProperty("bot_id")]
		public string BotId { get; set; }

		[JsonProperty("attachments")]
		public SlackAttachment[] Attachments { get; set; }
	}
}
