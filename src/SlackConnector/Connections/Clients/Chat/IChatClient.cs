using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    public interface IChatClient
    {
		Task<MessageResponse> PostMessage(string slackKey, string channel, string text, 
			IList<SlackAttachment> attachments, string threadTs = null, string iconUrl = null,
			string userName = null, bool asUser = false, bool linkNames = true);

		Task<MessageResponse> PostEphemeral(string slackKey, string channel, string user, string text, 
			IList<SlackAttachment> attachments, string threadTs = null, string iconUrl = null, 
			string userName = null, bool asUser = false, bool linkNames = true)

		Task Update(string slackKey, string messageTs, string channel, string text,
			IList<SlackAttachment> attachments, bool asUser = false, bool linkNames = true);

		Task Delete(string slackKey, string channel, string ts, bool asUser = false);
    }
}