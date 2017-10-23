using Newtonsoft.Json.Linq;
using System;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal class FileReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }
        public string File { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "file" )
            {
                type = ReactionItemType.file;
                File = reactionjObject["item"]["file"].Value<string>();
            }
            else
            {
                throw new ArgumentException(String.Format("ReactionObject type {0} is invalid for FileReaction", reactionjObject["item"]["type"].Value<string>()));
            }
        }
    }
}
