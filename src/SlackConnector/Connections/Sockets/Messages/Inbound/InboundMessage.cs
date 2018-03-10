using Newtonsoft.Json;
using SlackConnector.Serialising;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    //TOOD: Turn into interface?
    public abstract class InboundMessage
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(EnumConverter))]
        public MessageType MessageType { get; set; }

        public string RawData { get; set; }
    }
}
