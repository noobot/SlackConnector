namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal enum MessageType
    {
        Unknown = 0,
        Message,
        Group_Joined,
        Channel_Joined,
        Team_Join
    }
}
