using System.Threading.Tasks;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Logging;

namespace SlackConnector.Connections
{
    internal class ConnectionFactory : IConnectionFactory
    {
        public async Task<IWebSocketClient> CreateWebSocketClient(string url, ProxySettings proxySettings)
        {
            var socket = new WebSocketClientLite(new MessageInterpreter(new Logger()));
            await socket.Connect(url);
            return socket;
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new FlurlHandshakeClient(new ResponseVerifier());
        }

        public IChatClient CreateChatClient()
        {
            return new FlurlChatClient(new ResponseVerifier());
        }

        public IFileClient CreateFileClient()
        {
            return new FlurlFileClient(new ResponseVerifier());
        }

        public IChannelClient CreateChannelClient()
        {
            return new FlurlChannelClient(new ResponseVerifier());
        }
    }
}