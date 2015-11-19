using System;
using System.Threading.Tasks;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;

namespace SlackConnector.Connections.Sockets
{
    internal interface IWebSocketClient
    {
        bool IsAlive { get; }

        Task Connect();
        Task SendMessage(BaseMessage message);
        void Close();

        event EventHandler<InboundMessage> OnMessage;
        event EventHandler OnClose;
    }
}