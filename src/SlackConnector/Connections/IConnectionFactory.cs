using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Sockets;

namespace SlackConnector.Connections
{
    internal interface IConnectionFactory
    {
        IWebSocketClient CreateWebSocketClient(string url);
        IHandshakeClient CreateHandshakeClient();
        IChatMessenger CreateChatMessenger();
        IChannelClient CreateChannelMessenger();
    }
}