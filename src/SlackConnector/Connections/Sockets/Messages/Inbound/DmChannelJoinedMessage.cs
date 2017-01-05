using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class DmChannelJoinedMessage : InboundMessage
    {
        public DmChannelJoinedMessage()
        {
            MessageType = MessageType.Im_Created;
        }

        public Im Channel { get; set; }
    }
}
