using Newtonsoft.Json.Linq;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    interface IReactionItem
    {
        void ParseItem(JObject jsonObject);
        ReactionItemType type { get; set; }
    }
}
