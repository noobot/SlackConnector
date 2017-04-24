using Newtonsoft.Json;
using SlackConnector.Serialising;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class ChatMessage : InboundMessage
    {
        public ChatMessage()
        {
            MessageType = MessageType.Message;
        }

        [JsonProperty("subtype")]
        [JsonConverter(typeof(EnumConverter))]
        public MessageSubType MessageSubType { get; set; }

        public string Channel { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Team { get; set; }

        [JsonProperty("ts")]
        public double TimeStamp { get; set; }

        /// <summary>
        /// in case there is an update of a message, the original message will be stored here
        /// </summary>
        [JsonProperty("message")]
        public ChatMessage UpdatedMessage { get; set; }

        [JsonProperty("previous_message")]
        public ChatMessage PreviousMessage { get; set; }
    }
}
