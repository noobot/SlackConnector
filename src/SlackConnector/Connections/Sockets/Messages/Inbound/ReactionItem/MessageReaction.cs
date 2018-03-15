using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
	public class MessageReaction : IReactionItem
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
