using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
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
