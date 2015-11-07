using System;
using System.Threading.Tasks;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class WebSocketClientStub : IWebSocketClient
    {
        public bool IsAlive { get; }

        public event EventHandler<InboundMessage> OnMessage;
        public void RaiseOnMessage(InboundMessage message)
        {
            OnMessage.Invoke(this, message);
        }

        public event EventHandler OnClose;
        public Task Connect()
        {
            return Task.Factory.StartNew(() => { });
        }

        public void Close()
        {

        }
    }
}