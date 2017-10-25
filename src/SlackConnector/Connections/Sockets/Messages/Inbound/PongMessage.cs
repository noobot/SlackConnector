using System;
using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class PongMessage : InboundMessage
    {
        public PongMessage()
        {
            MessageType = MessageType.Pong;
        }

        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("reply_to")]
        public int ReplyTo { get; set; }
    }
}