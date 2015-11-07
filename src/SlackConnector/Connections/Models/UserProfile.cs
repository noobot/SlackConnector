using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class UserProfile
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("real_name")]
        public string RealName { get; set; }
        [JsonProperty("real_name_normalized")]
        public string RealNameNormalized { get; set; }
        public string Email { get; set; }
    }
}