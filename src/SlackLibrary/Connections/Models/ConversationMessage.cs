using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
	public class ConversationMessage
	{
		public partial class Reply
		{
			[JsonProperty("user")]
			public string User { get; set; }

			[JsonProperty("ts")]
			public string Timestamp { get; set; }
		}

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("thread_ts")]
		public string ThreadTimestamp { get; set; }

		[JsonProperty("reply_count")]
		public long? ReplyCount { get; set; }

		[JsonProperty("replies")]
		public Reply[] Replies { get; set; }

		[JsonProperty("subscribed")]
		public bool Subscribed { get; set; }

		[JsonProperty("last_read")]
		public string LastRead { get; set; }

		[JsonProperty("unread_count")]
		public long UnreadCount { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("parent_user_id")]
		public string ParentUserId { get; set; }
	}
}
