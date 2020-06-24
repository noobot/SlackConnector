using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Connections.Sockets.Messages.Outbound;
using SlackLibrary.Exceptions;
using Websocket.Client;

namespace SlackLibrary.Connections.Sockets
{
    internal class WebSocketClient : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private WebsocketClient _client;
        private readonly TimeSpan _reconnectTimeout = TimeSpan.FromSeconds(30);
        private int _currentMessageId;

        public WebSocketClient(IMessageInterpreter interpreter)
        {
            _interpreter = interpreter;
        }

        public bool IsAlive => _client.IsRunning;

        public async Task Connect(string webSocketUrl)
        {
            if (_client != null)
            {
                await Close();
            }

            var uri = new Uri(webSocketUrl);
            _client = new WebsocketClient(uri)
            {
                ReconnectTimeout = _reconnectTimeout,
                IsReconnectionEnabled = true
            };

            _client.MessageReceived.Subscribe(MessageReceived);
            _client.DisconnectionHappened.Subscribe(Disconnected);

            await _client.Start();
        }

        public Task SendMessage(BaseMessage message)
        {
            if (!IsAlive)
            {
                throw new CommunicationException("Connection not Alive");
            }

            System.Threading.Interlocked.Increment(ref _currentMessageId);
            message.Id = _currentMessageId;
            var json = JsonConvert.SerializeObject(message);

            _client.Send(json);

            return Task.CompletedTask;
        }

        public Task Close()
        {
            _client.Dispose();
            return Task.CompletedTask;
        }

        public event EventHandler OnClose;
        private void Disconnected(DisconnectionInfo obj)
        {
            OnClose?.Invoke(this, null);
        }

        public event EventHandler<InboundMessage> OnMessage;
        private void MessageReceived(ResponseMessage message)
        {
            string messageJson = message.Text ?? "";
            var inboundMessage = _interpreter.InterpretMessage(messageJson);
            OnMessage?.Invoke(this, inboundMessage);
        }
    }
}