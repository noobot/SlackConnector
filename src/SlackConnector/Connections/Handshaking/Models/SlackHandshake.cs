using Newtonsoft.Json;

namespace SlackConnector.Connections.Handshaking.Models
{
    internal class SlackHandshake
    {
        public bool Ok { get; set; }

        [JsonProperty("Url")]
        public string WebSocketUrl { get; set; } 

        public Detail Team { get; set; }
        public Detail Self { get; set; }
        public Detail[] Users { get; set; }
        public Channel[] Channels { get; set; }
        public Channel[] Groups { get; set; }
        public Im[] Ims { get; set; }
    }
}