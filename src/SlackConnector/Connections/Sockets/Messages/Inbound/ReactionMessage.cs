using Newtonsoft.Json;
using SlackConnector.Serialising;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class ReactionMessage : InboundMessage
    {
        public ReactionMessage()
        {
            MessageType = MessageType.Reaction_Added;
        }

        [JsonProperty("subtype")]
        [JsonConverter(typeof(EnumConverter))]
        public MessageSubType MessageSubType { get; set; }

        public string Channel { get; set; }
        public string User { get; set; }
        public string Reaction { get; set; }
        public double ReactingToTimestamp { get; set; }

        [JsonProperty("ts")]
        public double Timestamp { get; set; }
    }
}
