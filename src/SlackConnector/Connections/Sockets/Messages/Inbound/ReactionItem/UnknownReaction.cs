using Newtonsoft.Json.Linq;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal class UnknownReaction : IReactionItem
    {
        public ReactionItemType type { get; set; }

        public void ParseItem(JObject reactionjObject)
        {
            type = ReactionItemType.unknown;
        }
    }
}
