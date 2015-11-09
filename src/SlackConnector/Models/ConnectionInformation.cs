using System.Collections.Generic;
using SlackConnector.Connections.Sockets;

namespace SlackConnector.Models
{
    internal class ConnectionInformation
    {
        public ContactDetails Self { get; set; }
        public ContactDetails Team { get; set; }
        public Dictionary<string, string> Users { get; set; }
        public Dictionary<string, SlackChatHub> SlackChatHubs { get; set; }
        public IWebSocketClient WebSocket { get; set; }
    }
}