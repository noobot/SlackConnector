using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SlackConnector.Connections.Responses
{
    public class MessageObjectResponse
    {
        public string Type { get; set; }
        
        public string User { get; set; }
        
        public string Text { get; set; }

        [JsonProperty("bot_id")]
        public string BotId { get; set; }

        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
    }
}
