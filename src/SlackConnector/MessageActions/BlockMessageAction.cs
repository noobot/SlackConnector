using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class BlockInboundAction
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

	public class BlockMessageAction : InboundCommonMessageAction
	{
		[JsonProperty(PropertyName = "actions", NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<BlockInboundAction> Actions { get; set; }
	}
}
