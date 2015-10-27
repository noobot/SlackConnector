namespace SlackConnector.Connections.Sockets
{
    internal interface IWebSocketFactory
    {
        IWebSocket Create(string url);
    }
}