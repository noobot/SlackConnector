using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class ReactionMessage : InboundMessage
    {
        public ReactionMessage()
        {
            MessageType = MessageType.Reaction_Added;
        }
        
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
