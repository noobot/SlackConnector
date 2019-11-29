using System;
using System.Threading.Tasks;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Connections.Sockets.Messages.Outbound;

namespace SlackLibrary.Connections.Sockets
{
    public interface IWebSocketClient
    {
        bool IsAlive { get; }

        Task Connect(string webSockerUrl);
        Task SendMessage(BaseMessage message);
        Task Close();

        event EventHandler<InboundMessage> OnMessage;
        event EventHandler OnClose;
    }
}