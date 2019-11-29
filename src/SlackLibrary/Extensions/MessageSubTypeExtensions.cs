using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Models;

namespace SlackLibrary.Extensions
{
    internal static class MessageSubTypeExtensions
    {
        public static SlackMessageSubType ToSlackMessageSubType(this MessageSubType subType)
        {
            return (SlackMessageSubType)subType;
        }
    }
}