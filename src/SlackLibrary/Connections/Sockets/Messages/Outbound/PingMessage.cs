using System;

namespace SlackLibrary.Connections.Sockets.Messages.Outbound
{
    internal class PingMessage : BaseMessage
    {
        public DateTime Timestamp { get; } = DateTime.Now;

        public PingMessage()
        {
            Type = "ping";
        }
    }
}
