using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Messaging
{
    public interface IChatMessenger
    {
        Task PostMessage(string slackKey, string channel, string text, SlackAttachment[] attachments);
    }
}