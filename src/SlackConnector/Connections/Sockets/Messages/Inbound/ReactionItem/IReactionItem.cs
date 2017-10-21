using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    interface IReactionItem
    {
        void ParseItem(JObject jsonObject);
        ReactionItemType type { get; set; }
    }
}
