using System;
using Newtonsoft.Json.Linq;
using SlackConnector.Sockets.Messages;
using WebSocketSharp;

namespace SlackConnector.Sockets
{
    internal interface IWebSocket
    {
        bool IsAlive { get; }

        event EventHandler OnOpen;
        event EventHandler<InboundMessage> OnMessage;
        event EventHandler OnClose;

        void Connect();
        void Close();
    }
}