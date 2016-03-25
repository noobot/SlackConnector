using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;
using WebSocketSharp;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketClient : IWebSocketClient
    {
        private readonly IMessageInterpreter _interpreter;
        private readonly WebSocket _webSocket;
        private int _currentMessageId;

        public WebSocketClient(IMessageInterpreter interpreter, string url, ProxySettings proxySettings)
        {
            _interpreter = interpreter;

            _webSocket = new WebSocket(url);
            _webSocket.OnMessage += WebSocketOnMessage;
            _webSocket.OnClose += (sender, args) => OnClose?.Invoke(sender, args);
            _webSocket.Log.Level = SlackConnector.LoggingLevel == ConsoleLoggingLevel.FatalErrors ? LogLevel.Fatal : LogLevel.Trace;

            if (proxySettings != null)
            {
                _webSocket.SetProxy(proxySettings.Url, proxySettings.Username, proxySettings.Password);
            }
        }

        public bool IsAlive => _webSocket.IsAlive;

        public event EventHandler<InboundMessage> OnMessage;
        public event EventHandler OnClose;
        
        public async Task Connect()
        {
            var taskSource = new TaskCompletionSource<bool>();

            _webSocket.OnOpen += (sender, args) => { taskSource.SetResult(true); };
            _webSocket.OnError += (sender, args) => { taskSource.SetException(args.Exception); };
            _webSocket.Connect();
            await taskSource.Task;
        }

        public async Task SendMessage(BaseMessage message)
        {
            _currentMessageId++;
            message.Id = _currentMessageId;
            string json = JsonConvert.SerializeObject(message);

            var taskSource = new TaskCompletionSource<bool>();
            _webSocket.SendAsync(json, sent =>
            {
                if (sent)
                {
                    taskSource.SetResult(true);
                }
                else
                {
                    taskSource.SetException(new Exception("Error occured while sending message"));
                }
            });
            
            await taskSource.Task;
        }

        public void Close()
        {
            _webSocket.Close();
        }

        private void WebSocketOnMessage(object sender, MessageEventArgs args)
        {
            string messageJson = args?.Data ?? "";
            InboundMessage inboundMessage = _interpreter.InterpretMessage(messageJson);
            OnMessage?.Invoke(sender, inboundMessage);
        }
    }
}