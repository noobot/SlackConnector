using System;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class PongMessage : InboundMessage
    {
        public DateTime Timestamp { get; set; }

        public PongMessage()
        {
            MessageType = MessageType.Pong;
        }
    }
}