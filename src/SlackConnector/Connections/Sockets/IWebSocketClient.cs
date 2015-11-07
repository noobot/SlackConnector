using System;
using System.Threading.Tasks;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections.Sockets
{
    internal interface IWebSocketClient
    {
        bool IsAlive { get; }
        
        event EventHandler<InboundMessage> OnMessage;
        event EventHandler OnClose;

        Task Connect();
        void Close();
    }
}