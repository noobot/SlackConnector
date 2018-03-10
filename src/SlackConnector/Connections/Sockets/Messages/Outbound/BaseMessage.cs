using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Outbound
{
    public abstract class BaseMessage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}