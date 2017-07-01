using Newtonsoft.Json;

namespace SlackConnector.Connections.Responses
{
    internal class PostMessageResponse : StandardResponse
    {
        [JsonProperty("ts")]
        public double TimeStamp { get; set; }

        public string Channel { get; set; }

        /// <summary>
        /// Message object, as it was parsed by Slack servers
        /// </summary>
        public MessageObjectResponse Message { get; set; }
    }
}
