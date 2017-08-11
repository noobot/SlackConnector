using System;
using System.Threading.Tasks;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;

namespace SlackConnector.Tests.Unit.Stubs
{
    internal class WebSocketClientStub : IWebSocketClient
    {
        public bool IsAlive { get; set; }
        public int CurrentMessageId { get; set; }

        public event EventHandler<InboundMessage> OnMessage;
        public void RaiseOnMessage(InboundMessage message)
        {
            OnMessage.Invoke(this, message);
        }

        public event EventHandler OnClose;
        public void RaiseOnClose()
        {
            OnClose.Invoke(this, null);
        }

        public Task Connect(string webSockerUrl)
        {
            return Task.CompletedTask;
        }

        public Task Connect()
        {
            return Task.CompletedTask;
        }

        public BaseMessage SendMessage_Message { get; private set; }
        public Task SendMessage(BaseMessage message)
        {
            SendMessage_Message = message;
            return Task.CompletedTask;
        }
        
        public Task Close()
        {
            return Task.CompletedTask;
        }
    }
}