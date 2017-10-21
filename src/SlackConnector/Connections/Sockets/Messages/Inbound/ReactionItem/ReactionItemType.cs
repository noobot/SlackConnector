using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Sockets.Messages.Inbound.ReactionItem
{
    internal enum ReactionItemType
    {
        message,
        file,
        file_comment,
        unknown
    }
}
