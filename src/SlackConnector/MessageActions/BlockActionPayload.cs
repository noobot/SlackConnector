using Newtonsoft.Json;
using SlackConnector.EventAPI;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class SelectInboundBlockAction : InboundBlockAction
	{
		[JsonProperty(PropertyName = "selected_option", NullValueHandling = NullValueHandling.Ignore)]
		public OptionObject SelectedOption { get; set; }
	}

	public class InboundBlockAction
	{
		[JsonProperty(PropertyName = "block_id", NullValueHandling = NullValueHandling.Ignore)]
		public string BlockId { get; set; }

		[JsonProperty(PropertyName = "action_id", NullValueHandling = NullValueHandling.Ignore)]
		public string ActionId { get; set; }

		[JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
		public string Value { get; set; }

		[JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
		public string Type { get; set; }
	}

	public class BlockActionPayload : CommonActionPayload
	{
		[JsonProperty(PropertyName = "actions", NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<InboundBlockAction> Actions { get; set; }

		[JsonProperty("message")]
		public MessageEvent Message { get; set; }
	}
}
