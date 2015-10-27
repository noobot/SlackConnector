using Newtonsoft.Json;

namespace SlackConnector.Models.Handshake
{
    internal class User : Detail
    {
        [JsonProperty("real_name")]
        public string RealName { get; set; }
        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }
        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }
        public bool Deleted { get; set; }
    }
}