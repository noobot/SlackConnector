using System;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace SlackConnector.Sockets
{
    internal interface IWebSocket
    {
        bool IsAlive { get; }

        event EventHandler OnOpen;
        event EventHandler<JObject> OnMessage;
        event EventHandler OnClose;

        void Connect();
        void Close();
    }
}