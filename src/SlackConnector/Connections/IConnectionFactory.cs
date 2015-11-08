using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Sockets;

namespace SlackConnector.Connections
{
    internal interface IConnectionFactory
    {
        IWebSocketClient CreateWebSocketClient(string url);
        IHandshakeClient CreateHandshakeClient();
        IChatMessenger CreateChatMessenger();
        IChannelMessenger CreateChannelMessenger();
    }
}