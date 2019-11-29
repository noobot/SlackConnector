using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
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
