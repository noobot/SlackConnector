namespace SlackLibrary.Connections.Sockets.Messages.Inbound
{
    internal interface IMessageInterpreter
    {
        InboundMessage InterpretMessage(string json);
    }
}