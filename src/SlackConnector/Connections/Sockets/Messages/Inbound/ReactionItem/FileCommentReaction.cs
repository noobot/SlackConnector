using Newtonsoft.Json.Linq;
using System;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal class FileCommentReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }
        public string File { get; set; }
        public string FileComment { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            if (reactionjObject["item"]["type"].Value<string>() == "file_comment")
            {
               type = ReactionItemType.file_comment;
               File = reactionjObject["item"]["file"].Value<string>();
               FileComment = reactionjObject["item"]["file_comment"].Value<string>();
            }
            else
            {
                throw new ArgumentException($"ReactionObject type {reactionjObject["item"]["type"].Value<string>()} is invalid for FileReaction");
            }
        }
    }
}
