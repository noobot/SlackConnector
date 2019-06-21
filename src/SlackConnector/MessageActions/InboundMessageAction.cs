using Newtonsoft.Json;
using SlackConnector.EventAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class InboundMessageAction : InboundCommonMessageAction
	{
		public partial class InboundActionSelectedOption
		{
			[JsonProperty("value")]
			public string Value { get; set; }
		}

		public partial class InboundButtonAction : InboundAction
		{
			[JsonProperty("value")]
			public string Value { get; set; }
		}

		public partial class InboundOptionsAction : InboundAction
		{
			[JsonProperty("selected_options")]
			public InboundActionSelectedOption[] SelectedOptions { get; set; }
		}

		public partial class InboundAction
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }
		}

		[JsonProperty("actions")]
		public InboundAction[] Actions { get; set; }

		[JsonProperty("callback_id")]
		public string CallbackId { get; set; }

		[JsonProperty("action_ts")]
		public string ActionTimestamp { get; set; }

		[JsonProperty("message_ts")]
		public string MessageTimestamp { get; set; }

		[JsonProperty("attachment_id")]
		public string AttachmentId { get; set; }


		[JsonProperty("original_message")]
		public MessageEvent OriginalMessage { get; set; }

		[JsonProperty("trigger_id")]
		public string TriggerId { get; set; }

		public string RawJson { get; set; }
	}
}
