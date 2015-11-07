using System;
using System.Threading.Tasks;
using SlackConnector.Connections.Sockets.Messages;

namespace SlackConnector.Connections.Sockets
{
    internal class WebSocketClient : IWebSocketClient
    {
        private readonly WebSocketSharp.WebSocket _webSocket;

        public WebSocketClient(IMessageInterpreter interpreter, string url)
        {
            _webSocket = new WebSocketSharp.WebSocket(url);
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

        public void Close()
        {
            _webSocket.Close();
        }
    }
}