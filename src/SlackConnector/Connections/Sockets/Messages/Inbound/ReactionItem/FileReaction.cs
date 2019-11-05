using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class FileReaction : IReactionItem
    {
		public FileReaction()
		{
			Type = "file";
		}

		[JsonProperty("type")]
		public string Type { get; set; }

		public string File { get; set; }
    }
}
