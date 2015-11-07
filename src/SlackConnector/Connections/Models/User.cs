using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class User : Detail
    {
        public bool Deleted { get; set; }

        public Profile Profile { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }
    }
}