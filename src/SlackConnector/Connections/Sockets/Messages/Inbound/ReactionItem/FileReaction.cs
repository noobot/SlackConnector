using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class FileReaction : IReactionItem
    {
		[JsonProperty("type")]
		public string Type { get; set; }

		public string File { get; set; }
    }
}
