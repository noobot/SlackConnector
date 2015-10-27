namespace SlackConnector.Connections.Sockets
{
    internal interface IWebSocketFactory
    {
        IWebSocketClient Create(string url);
    }
}