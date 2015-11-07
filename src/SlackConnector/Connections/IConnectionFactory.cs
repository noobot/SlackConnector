using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Sockets;

namespace SlackConnector.Connections
{
    internal interface IConnectionFactory
    {
        IWebSocketClient CreateWebSocketClient(string url);
        IHandshakeClient CreateHandshakeClient();
    }
}