using System;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocket : IWebSocket
    {
        private readonly WebSocketSharp.WebSocket _webSocket;

        public WebSocket(IMessageInterpreter interpreter, string url)
        {
            _webSocket = new WebSocketSharp.WebSocket(url);
            _webSocket.OnOpen += (sender, args) => OnOpen?.Invoke(sender, args);
            _webSocket.OnMessage += (sender, args) => OnMessage?.Invoke(sender, interpreter.InterpretMessage(args?.Data ?? ""));
            _webSocket.OnClose += (sender, args) => OnClose?.Invoke(sender, args);
        }

        public bool IsAlive => _webSocket.IsAlive;

        public event EventHandler OnOpen;
        public event EventHandler<InboundMessage> OnMessage;
        public event EventHandler OnClose;

        public void Connect()
        {
            _webSocket.Connect();
        }

        public void Close()
        {
            _webSocket.Close();
        }
    }
}