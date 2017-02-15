using System;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class PongMessage : InboundMessage
    {
        public DateTime TimeStamp { get; set; }
    }
}