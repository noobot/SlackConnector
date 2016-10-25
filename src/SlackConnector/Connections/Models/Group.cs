using Newtonsoft.Json;

namespace SlackConnector.Connections.Models
{
    internal class Group : Detail
    {
        [JsonProperty("is_group")]
        public bool IsGroup { get; set; }
        
        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }

        [JsonProperty("members")]
        public string[] Members { get; set; }
    }
}