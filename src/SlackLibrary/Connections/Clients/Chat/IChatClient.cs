using System.Collections.Generic;
using System.Threading.Tasks;
using SlackLibrary.Connections.Responses;
using SlackLibrary.Models;
using SlackLibrary.Models.Blocks;

namespace SlackLibrary.Connections.Clients.Chat
{
    public interface IChatClient
    {
		Task<MessageResponse> PostMessage(string slackKey, string channel, string text,
			IEnumerable<SlackAttachment> attachments = null, string threadTs = null, string iconUrl = null,
			string userName = null, bool asUser = false, bool linkNames = true, 
			IEnumerable<BlockBase> blocks = null);

		Task<MessageResponse> PostEphemeral(string slackKey, string channel, string user, string text,
			IEnumerable<SlackAttachment> attachments = null, string threadTs = null, string iconUrl = null,
			string userName = null, bool asUser = false, bool linkNames = true, 
			IEnumerable<BlockBase> blocks = null);

		Task Update(string slackKey, string messageTs, string channel, string text,
			IEnumerable<SlackAttachment> attachments = null, bool asUser = false, bool linkNames = true, 
			IEnumerable<BlockBase> blocks = null);

		Task Update(string responseUrl, string text, IEnumerable<SlackAttachment> attachments = null, IEnumerable<BlockBase> blocks = null);

		Task Delete(string slackKey, string channel, string ts, bool asUser = false);

        Task DeleteOriginalMessage(string responseUrl);
    }
}