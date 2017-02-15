using System;

namespace SlackConnector.Connections.Sockets.Messages.Outbound
{
    internal class PingMessage : BaseMessage
    {
        public DateTime TimeStamp { get; } = DateTime.Now;

        public PingMessage()
        {
            Type = "ping";
        }
    }
}
