using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
{
    internal class ChannelJoinedMessage : InboundMessage
    {
        public ChannelJoinedMessage()
        {
            MessageType = MessageType.Channel_Joined;
        }

        public Channel Channel { get; set; }
    }
}
