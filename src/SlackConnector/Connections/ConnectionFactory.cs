using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections
{
    internal class ConnectionFactory : IConnectionFactory
    {
        public IWebSocketClient CreateWebSocketClient(string url)
        {
            return new WebSocketClient(new MessageInterpreter(), url);
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new HandshakeClient(new RestSharpFactory());
        }
    }
}