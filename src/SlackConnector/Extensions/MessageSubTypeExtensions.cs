using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;

namespace SlackConnector.Extensions
{
    internal static class MessageSubTypeExtensions
    {
        public static SlackMessageSubType ToSlackMessageSubType(this MessageSubType subType)
        {
            return (SlackMessageSubType)subType;
        }
    }
}