using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    class UnknownReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            this.type = ReactionItemType.unknown;
        }
    }
}
