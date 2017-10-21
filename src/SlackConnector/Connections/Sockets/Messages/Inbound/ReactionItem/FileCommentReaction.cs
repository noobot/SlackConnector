using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    class FileCommentReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }
        public string File { get; set; }
        public string FileComment { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "file_comment" )
            {
                this.type = ReactionItemType.file_comment;
                this.File = reactionjObject["item"]["file"].Value<string>();
                this.FileComment = reactionjObject["item"]["file_comment"].Value<string>();
            }
            else
            {
                throw new ArgumentException(String.Format("ReactionObject type {0} is invalid for FileReaction", reactionjObject["item"]["type"].Value<string>()));
            }
        }
    }
}
