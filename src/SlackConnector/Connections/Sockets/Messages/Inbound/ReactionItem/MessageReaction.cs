using Newtonsoft.Json.Linq;
using System;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal class MessageReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }
        public string Channel { get; set; }
        public double Timestamp { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "message" )
            {
                type = ReactionItemType.message;
                Channel = reactionjObject["item"]["channel"].Value<string>();
                Timestamp = reactionjObject["item"]["ts"].Value<double>();
            }
            else
            {
                throw new ArgumentException(String.Format("ReactionObject type {0} is invalid for MessageReaction", reactionjObject["item"]["type"].Value<string>()));
            }
        }
    }
}
