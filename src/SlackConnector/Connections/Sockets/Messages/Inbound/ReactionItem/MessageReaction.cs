using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class MessageReaction : IReactionItem
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

		[JsonProperty("ts")]
		public string Timestamp { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
