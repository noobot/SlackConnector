using Newtonsoft.Json;

namespace SlackConnector.Models.Handshake
{
    internal class Channel : Detail
    {
        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_member")]
        public bool IsMember { get; set; }  
    }
}