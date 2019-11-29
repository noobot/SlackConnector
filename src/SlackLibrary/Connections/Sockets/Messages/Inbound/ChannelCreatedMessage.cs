using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
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