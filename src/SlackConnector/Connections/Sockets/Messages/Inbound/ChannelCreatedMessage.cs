using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class ChannelCreatedMessage : InboundMessage
    {
        public ChannelCreatedMessage()
        {
            MessageType = MessageType.Channel_Created;
        }

        public Channel Channel { get; set; }
    }
}