using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    class FileReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }
        public string File { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "file" )
            {
                this.type = ReactionItemType.file;
                this.File = reactionjObject["item"]["file"].Value<string>();
            }
            else
            {
                throw new ArgumentException(String.Format("ReactionObject type {0} is invalid for FileReaction", reactionjObject["item"]["type"].Value<string>()));
            }
        }
    }
}
