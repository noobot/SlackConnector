using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketFactory : IWebSocketFactory
    {
        public IWebSocket Create(string url)
        {
            return new WebSocket(new MessageInterpreter(), url);
        }
    }
}