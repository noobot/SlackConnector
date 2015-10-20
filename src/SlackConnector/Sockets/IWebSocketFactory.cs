namespace SlackConnector.Sockets
{
    internal interface IWebSocketFactory
    {
        IWebSocket Create(string url);
    }
}