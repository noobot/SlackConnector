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
        private readonly IRequestExecutor _requestExecutor;

        public ConnectionFactory()
        {
            IRestSharpFactory restSharpFactory = new RestSharpFactory();
            IResponseVerifier responseVerifier = new ResponseVerifier();
            _requestExecutor = new RequestExecutor(restSharpFactory, responseVerifier);
        }

        public IWebSocketClient CreateWebSocketClient(string url, ProxySettings proxySettings)
        {
            return new WebSocketClient(new MessageInterpreter(new Logger()), url, proxySettings);
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new HandshakeClient(_requestExecutor);
        }

        public IChatClient CreateChatClient()
        {
            return new ChatClient(_requestExecutor);
        }

        public IFileClient CreateFileClient()
        {
            return new FileClient(_requestExecutor);
        }

        public IChannelClient CreateChannelClient()
        {
            return new ChannelClient(_requestExecutor);
        }
    }
}