namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class UnknownMessage : InboundMessage
    {
        public UnknownMessage()
        {
            MessageType = MessageType.Unknown;
        }
    }
}