using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    public interface IChatClient
    {
        Task PostMessage(string slackKey, string channel, string text, 
			IList<SlackAttachment> attachments, string threadTs = null, string iconUrl = null,
			string userName = null, bool asUser = false, bool linkNames = true);

		Task Update(string slackKey, string messageTs, string channel, string text,
			IList<SlackAttachment> attachments, bool asUser = false, bool linkNames = true);
    }
}