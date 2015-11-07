using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class Im
    {
        public string Id { get; set; }

        public string User { get; set; }

        [JsonProperty("is_im")]
        public bool IsIm { get; set; }

        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }
    }
}