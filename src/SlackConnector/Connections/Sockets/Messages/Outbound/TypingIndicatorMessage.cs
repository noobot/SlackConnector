using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Outbound
{
    internal class TypingIndicatorMessage : BaseMessage
    {
        [JsonProperty("channel")]
        public string Channel { get; set; } 
    }
}