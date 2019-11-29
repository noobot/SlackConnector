using Newtonsoft.Json;
using SlackLibrary.Serialising;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
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
