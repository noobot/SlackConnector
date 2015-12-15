using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;

namespace SlackConnector.Connections
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly IRestSharpFactory _restSharpFactory;
        private readonly IResponseVerifier _responseVerifier;

        public ConnectionFactory()
        {
            _restSharpFactory = new RestSharpFactory();
            _responseVerifier = new ResponseVerifier();
        }

        public IWebSocketClient CreateWebSocketClient(string url)
        {
            return new WebSocketClient(new MessageInterpreter(), url);
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new HandshakeClient(_restSharpFactory, _responseVerifier);
        }

        public IChatMessenger CreateChatMessenger()
        {
            return new ChatMessenger(_restSharpFactory, _responseVerifier);
        }

        public IChannelMessenger CreateChannelMessenger()
        {
            return new ChannelMessenger(_restSharpFactory, _responseVerifier);
        }
    }
}