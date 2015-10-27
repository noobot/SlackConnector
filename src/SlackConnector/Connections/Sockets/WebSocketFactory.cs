using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketFactory : IWebSocketFactory
    {
        public IWebSocketClient Create(string url)
        {
            return new WebSocketClient(new MessageInterpreter(), url);
        }
    }
}