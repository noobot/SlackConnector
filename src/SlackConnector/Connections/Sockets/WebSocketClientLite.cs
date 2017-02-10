using System;
using System.Threading.Tasks;
using IWebsocketClientLite.PCL;
using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using WebsocketClientLite.PCL;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketClientLite : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private readonly IMessageWebSocketRx _webSocket;
        private readonly Uri _uri;
        private int _currentMessageId;

        public bool IsAlive => _webSocket.IsConnected;

        public WebSocketClientLite(IMessageInterpreter interpreter, string url)
        {
            _interpreter = interpreter;
            _uri = new Uri(url);

            _webSocket = new MessageWebSocketRx();
            _webSocket.ObserveTextMessagesReceived.Subscribe(OnWebSocketOnMessage);
            _webSocket.ObserveConnectionStatus.Subscribe(OnConnectionChange);
        }

        public async Task Connect()
        {
            await _webSocket.ConnectAsync(_uri);
        }

        public async Task SendMessage(BaseMessage message)
        {
            System.Threading.Interlocked.Increment(ref _currentMessageId);
            message.Id = _currentMessageId;
            string json = JsonConvert.SerializeObject(message);

            await _webSocket.SendTextAsync(json);
        }

        public async Task Close()
        {
            await _webSocket.CloseAsync();
        }

        public event EventHandler<InboundMessage> OnMessage;
        private void OnWebSocketOnMessage(string message)
        {
            string messageJson = message ?? "";
            var inboundMessage = _interpreter.InterpretMessage(messageJson);
            OnMessage?.Invoke(this, inboundMessage);
        }

        public event EventHandler OnClose;
        private void OnConnectionChange(ConnectionStatus connectionStatus)
        {
            switch (connectionStatus)
            {
                case ConnectionStatus.Aborted:
                case ConnectionStatus.ConnectionFailed:
                case ConnectionStatus.Disconnected:
                    OnClose?.Invoke(this, null);
                    break;
            }
        }
    }
}