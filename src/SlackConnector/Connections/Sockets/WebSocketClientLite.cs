using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
        private IMessageWebSocketRx _webSocket;
        private int _currentMessageId;

        public bool IsAlive => _webSocket.IsConnected;

        public WebSocketClientLite(IMessageInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public async Task Connect(string webSockerUrl)
        {
            if (_webSocket != null)
            {
                await Close();
            }

            _webSocket = new MessageWebSocketRx();
            _subscriptions.Add(_webSocket.ObserveConnectionStatus.Subscribe(OnConnectionChange));

            var uri = new Uri(webSockerUrl);
            var messageObserver = await _webSocket.CreateObservableMessageReceiver(uri, excludeZeroApplicationDataInPong: true);
            _subscriptions.Add(messageObserver.Subscribe(OnWebSocketOnMessage));
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
            using (_webSocket)
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription.Dispose();
                }
                _subscriptions.Clear();

                await _webSocket.CloseAsync();
            }
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