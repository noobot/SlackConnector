namespace SlackLibrary.Connections.Sockets.Messages.Inbound
{
    public enum MessageType
    {
        Unknown = 0,
        Message,
        Group_Joined,
        Channel_Joined,
        Im_Created,
        Team_Join,
        Pong,
        Reaction_Added,
        Channel_Created,
        Presence_Change
    }
}
