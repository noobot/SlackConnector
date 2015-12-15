using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Sockets;

namespace SlackConnector.Connections
{
    internal interface IConnectionFactory
    {
        IWebSocketClient CreateWebSocketClient(string url);
        IHandshakeClient CreateHandshakeClient();
        IChatClient CreateChatMessenger();
        IChannelClient CreateChannelMessenger();
    }
}