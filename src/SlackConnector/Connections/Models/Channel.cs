using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class Channel : Detail
    {
        [JsonProperty("is_channel")]
        public bool IsChannel { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_member")]
        public bool IsMember { get; set; }

        [JsonProperty("members")]
        public string[] Members { get; set; }

        public string Creator { get; set; }
    }
}