using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SlackConnector.Connections.Responses
{
    internal class DeleteMessageResponse : StandardResponse
    {
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }

        public string Channel { get; set; }
    }
}
