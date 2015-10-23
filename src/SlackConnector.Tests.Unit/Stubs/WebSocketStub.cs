using System;
using SlackConnector.Sockets;
using SlackConnector.Sockets.Messages;

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