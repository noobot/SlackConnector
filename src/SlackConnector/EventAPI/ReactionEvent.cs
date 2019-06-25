using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class ReactionEvent : InboundEvent
    {
		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("reaction")]
		public string Reaction { get; set; }

		[JsonProperty("event_ts")]
		public double Timestamp { get; set; }

		public IReactionItem ReactingTo { get; set; }

		[JsonProperty("item_user")]
		public string ReactingToUser { get; set; }
	}
}
