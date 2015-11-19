using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;
using SlackConnector.Connections.Sockets.Messages.Inbound;

namespace SlackConnector.Connections
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly IRestSharpFactory _restSharpFactory;

        public ConnectionFactory()
        {
            _restSharpFactory = new RestSharpFactory();
        }

        public IWebSocketClient CreateWebSocketClient(string url)
        {
            return new WebSocketClient(new MessageInterpreter(), url);
        }

        public IHandshakeClient CreateHandshakeClient()
        {
            return new HandshakeClient(_restSharpFactory);
        }

        public IChatMessenger CreateChatMessenger()
        {
            return new ChatMessenger(_restSharpFactory);
        }

        public IChannelMessenger CreateChannelMessenger()
        {
            return new ChannelMessenger(_restSharpFactory);
        }
    }
}