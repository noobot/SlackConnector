namespace SlackConnector.Sockets.Messages
{
    internal interface IMessageInterpreter
    {
        InboundMessage InterpretMessage(string json);
    }
}