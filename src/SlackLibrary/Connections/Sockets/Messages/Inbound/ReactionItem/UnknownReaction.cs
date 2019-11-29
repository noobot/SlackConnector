using Newtonsoft.Json;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class UnknownReaction : IReactionItem
    {
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
