using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Outbound
{
    internal class PingMessage : BaseMessage
    {
        public PingMessage()
        {
            Type = "ping";
        }
    }
}
