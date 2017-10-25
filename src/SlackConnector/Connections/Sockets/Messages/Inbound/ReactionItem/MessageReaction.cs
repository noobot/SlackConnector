using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal class MessageReaction : IReactionItem
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
