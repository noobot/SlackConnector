using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients
{
    internal interface IChatClient
    {
        Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments);
    }
}