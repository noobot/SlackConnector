using Newtonsoft.Json;

namespace SlackConnector.Connections.Handshaking.Models
{
    internal class Channel : Detail
    {
        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_member")]
        public bool IsMember { get; set; }  
    }
}