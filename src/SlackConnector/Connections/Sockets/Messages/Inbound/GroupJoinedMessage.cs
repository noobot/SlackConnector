using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class GroupJoinedMessage : InboundMessage
    {
        public GroupJoinedMessage()
        {
            MessageType = MessageType.Group_Joined;
        }

        public Group Channel { get; set; }
    }
}
