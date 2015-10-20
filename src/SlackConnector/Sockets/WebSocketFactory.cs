namespace SlackConnector.Sockets
{
    internal class WebSocketFactory : IWebSocketFactory
    {
        public IWebSocket Create(string url)
        {
            return new WebSocket(url);
        }
    }
}