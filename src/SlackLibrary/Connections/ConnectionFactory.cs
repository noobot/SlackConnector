using System.Threading.Tasks;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Channel;
using SlackLibrary.Connections.Clients.Chat;
using SlackLibrary.Connections.Clients.Conversation;
using SlackLibrary.Connections.Clients.File;
using SlackLibrary.Connections.Clients.Handshake;
using SlackLibrary.Connections.Clients.Users;
using SlackLibrary.Connections.Sockets;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Logging;

namespace SlackLibrary.Connections
{
    public class ConnectionFactory : IConnectionFactory
    {
        public async Task<IWebSocketClient> CreateWebSocketClient(string url)
        {
            var socket = new WebSocketClient(new MessageInterpreter(new Logger()));
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

		public IConversationClient CreateConversationClient()
		{
			return new FlurlConversationClient(new ResponseVerifier());
		}

		public IUserClient CreateUserClient()
		{
			return new FlurlUserClient(new ResponseVerifier());
		}
	}
}