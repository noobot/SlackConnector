using Newtonsoft.Json;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class MessageReaction : IReactionItem
    {
		public MessageReaction()
		{
			this.Type = "message";
		}

		[JsonProperty("channel")]
        public string Channel { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
