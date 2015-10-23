using SlackConnector.Sockets.Messages;

namespace SlackConnector.Sockets
{
    internal class WebSocketFactory : IWebSocketFactory
    {
        private readonly IMessageInterpreter _messageInterpreter;

        public WebSocketFactory(IMessageInterpreter messageInterpreter)
        {
            _messageInterpreter = messageInterpreter;
        }

        public IWebSocket Create(string url)
        {
            return new WebSocket(_messageInterpreter ,url);
        }
    }
}