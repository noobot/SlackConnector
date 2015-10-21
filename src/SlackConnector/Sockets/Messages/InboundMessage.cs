using System;
using Newtonsoft.Json;

namespace SlackConnector.Sockets.Messages
{
    internal class InboundMessage
    {
        [JsonProperty("type")]
        public MessageType MessageType { get; set; }
        [JsonProperty("subtype")]
        public MessageSubType MessageSubType { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Team { get; set; }
    }
}