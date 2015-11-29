using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Connections.Sockets.Messages.Outbound;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketClient : IWebSocketClient
    {
        private readonly WebSocketSharp.WebSocket _webSocket;
        private int _currentMessageId = 0;

        public WebSocketClient(IMessageInterpreter interpreter, string url)
        {
            _webSocket = new WebSocketSharp.WebSocket(url);
            _webSocket.Log.Level = WebSocketSharp.LogLevel.Warn;
            _webSocket.OnMessage += (sender, args) => OnMessage?.Invoke(sender, interpreter.InterpretMessage(args?.Data ?? ""));
            _webSocket.OnClose += (sender, args) => OnClose?.Invoke(sender, args);
        }

        public bool IsAlive => _webSocket.IsAlive;

        public event EventHandler<InboundMessage> OnMessage;
        public event EventHandler OnClose;
        
        public async Task Connect()
        {
            var taskSource = new TaskCompletionSource<bool>();

            _webSocket.OnOpen += (sender, args) => { taskSource?.SetResult(true); };
            _webSocket.OnError += (sender, args) => { taskSource?.SetException(args.Exception); };
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
    }
}