using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    public class FileCommentReaction : IReactionItem
    {
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("file_comment")]
        public string FileComment { get; set; }
    }
}
