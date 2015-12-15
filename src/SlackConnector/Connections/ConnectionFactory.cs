using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;

namespace SlackConnector.Connections
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly IRestSharpFactory _restSharpFactory;
        private readonly IResponseVerifier _responseVerifier;
        private readonly IRequestExecutor _requestExecutor;

        public ConnectionFactory()
        {
            _restSharpFactory = new RestSharpFactory();
            _responseVerifier = new ResponseVerifier();
            _requestExecutor = new RequestExecutor(_restSharpFactory, _responseVerifier);
        }

        public IWebSocketClient CreateWebSocketClient(string url)
        {
            return new WebSocketClient(new MessageInterpreter(), url);
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new HandshakeClient(_restSharpFactory, _responseVerifier);
        }

        public IChatClient CreateChatClient()
        {
            return new ChatClient(_requestExecutor);
        }

        public IChannelClient CreateChannelClient()
        {
            return new ChannelClient(_requestExecutor);
        }
    }
}