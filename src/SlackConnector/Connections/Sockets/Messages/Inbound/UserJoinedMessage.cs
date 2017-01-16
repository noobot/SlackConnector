using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class UserJoinedMessage : InboundMessage
    {
        public UserJoinedMessage()
        {
            MessageType = MessageType.Team_Join;
        }

        public User User { get; set; }
    }
}
