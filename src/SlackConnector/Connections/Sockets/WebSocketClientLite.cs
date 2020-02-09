using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IWebsocketClientLite.PCL;
using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using SlackConnector.Exceptions;
using WebsocketClientLite.PCL;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketClientLite : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
        private MessageWebSocketRx _webSocket;
        private int _currentMessageId;
        private readonly TimeSpan _establishConnectionTimeout = TimeSpan.FromSeconds(20);
        private readonly TimeSpan _establishConnectionCheckDelay = TimeSpan.FromMilliseconds(10);

        public bool IsAlive { get; private set; }

        public WebSocketClientLite(IMessageInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public async Task Connect(string webSocketUrl)
        {
            if (_webSocket != null)
            {
                await Close();
            }

            _webSocket = new MessageWebSocketRx
            {
                ExcludeZeroApplicationDataInPong = true
            };

            _subscriptions.Add(_webSocket.ConnectionStatusObservable.Subscribe(OnConnectionChange));
            _subscriptions.Add(_webSocket.MessageReceiverObservable.Subscribe(OnWebSocketOnMessage));

            var uri = new Uri(webSocketUrl);

            await ConnectAndWait(_webSocket, uri);
        }

        private async Task ConnectAndWait(MessageWebSocketRx webSocket, Uri uri)
        {
            var currentStatus = ConnectionStatus.Disconnected;

            var connectChangeHandler = _webSocket.ConnectionStatusObservable.Subscribe(
                status => currentStatus = status);

            using (connectChangeHandler)
            {
                await _webSocket.ConnectAsync(uri);

                var timeout = DateTime.Now.Add(_establishConnectionTimeout);
                while (DateTime.Now < timeout)
                {
                    if (currentStatus == ConnectionStatus.HandshakeCompletedSuccessfully)
                    {
                        IsAlive = true;

                        await Task.Delay(_establishConnectionCheckDelay);
                        return;
                    }

                    await Task.Delay(_establishConnectionCheckDelay);
                }

                throw new ConnectionTimeout("Unable to establish connection to Slack servers");
            }
        }

        public async Task SendMessage(BaseMessage message)
        {
            if (!IsAlive)
            {
                throw new CommunicationException("Connection not Alive");
            }

            System.Threading.Interlocked.Increment(ref _currentMessageId);
            message.Id = _currentMessageId;
            var json = JsonConvert.SerializeObject(message);

            await _webSocket.SendTextAsync(json);
        }

        public async Task Close()
        {
            try
            {
                using (_webSocket)
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Dispose();
                    }
                    _subscriptions.Clear();

                    await _webSocket.DisconnectAsync();
                }
            }
            finally
            {
                IsAlive = false;
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