using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class UnknownReaction : IReactionItem
    {
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
