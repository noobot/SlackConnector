namespace SlackConnector.Connections.Sockets.Messages
{
    internal interface IMessageInterpreter
    {
        InboundMessage InterpretMessage(string json);
    }
}