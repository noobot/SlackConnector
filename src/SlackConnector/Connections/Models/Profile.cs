using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class Profile
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("real_name")]
        public string RealName { get; set; }

        [JsonProperty("real_name_normalized")]
        public string RealNameNormalised { get; set; }

        [JsonProperty("image_512")]
        public string Image { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        [JsonProperty("status_text")]
        public string StatusText { get; set;  }
    }
}