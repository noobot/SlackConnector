using Newtonsoft.Json;
using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class HandshakeResponse : StandardResponse
    {
        [JsonProperty("Url")]
        public string WebSocketUrl { get; set; } 

        public Detail Team { get; set; } = new Detail();
        public Detail Self { get; set; } = new Detail();
        public User[] Users { get; set; } = new User[0];
        public Channel[] Channels { get; set; } = new Channel[0];
        public Group[] Groups { get; set; } = new Group[0];
        public Im[] Ims { get; set; } = new Im[0];
    }
}