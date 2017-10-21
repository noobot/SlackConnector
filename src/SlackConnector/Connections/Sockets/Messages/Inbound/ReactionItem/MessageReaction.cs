using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    class MessageReaction : IReactionItem
    {

        public ReactionItemType type { get; set; }
        public string Channel { get; set; }
        public double Timestamp { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "message" )
            {
                this.type = ReactionItemType.message;
                this.Channel = reactionjObject["item"]["channel"].Value<string>();
                this.Timestamp = reactionjObject["item"]["ts"].Value<double>();
            }
            else
            {
                throw new ArgumentException(String.Format("ReactionObject type {0} is invalid for MessageReaction", reactionjObject["item"]["type"].Value<string>()));
            }
        }
    }
}
