using Newtonsoft.Json;

namespace SlackConnector.Sockets.Messages
{
    internal class InboundMessage
    {
        public MessageType MessageType { get; set; }
        public MessageSubType MessageSubType { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        [JsonProperty("ts")]
        public string Timestamp { get; set; }
        public string Team { get; set; }
    }
}