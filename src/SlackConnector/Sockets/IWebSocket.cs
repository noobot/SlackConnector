using System;
using WebSocketSharp;

namespace SlackConnector.Sockets
{
    internal interface IWebSocket
    {
        bool IsAlive { get; }

        event EventHandler OnOpen;
        event EventHandler<MessageEventArgs> OnMessage;
        event EventHandler<CloseEventArgs> OnClose;

        void Connect();
        void Close();
    }
}