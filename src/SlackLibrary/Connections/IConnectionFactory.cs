using System.Threading.Tasks;
using SlackLibrary.Connections.Clients.Channel;
using SlackLibrary.Connections.Clients.Chat;
using SlackLibrary.Connections.Clients.Conversation;
using SlackLibrary.Connections.Clients.File;
using SlackLibrary.Connections.Clients.Handshake;
using SlackLibrary.Connections.Clients.Users;
using SlackLibrary.Connections.Sockets;

namespace SlackLibrary.Connections
{
    internal interface IConnectionFactory
    {
        Task<IWebSocketClient> CreateWebSocketClient(string url, ProxySettings proxySettings);
        IHandshakeClient CreateHandshakeClient();
        IChatClient CreateChatClient();
        IFileClient CreateFileClient();
        IChannelClient CreateChannelClient();
		IConversationClient CreateConversationClient();
		IUserClient CreateUserClient();
	}
}