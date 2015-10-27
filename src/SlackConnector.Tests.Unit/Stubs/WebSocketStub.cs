using System;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class WebSocketStub : IWebSocket
    {
        public bool IsAlive { get; }
        public event EventHandler OnOpen;
        public event EventHandler<InboundMessage> OnMessage;
        public event EventHandler OnClose;
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}