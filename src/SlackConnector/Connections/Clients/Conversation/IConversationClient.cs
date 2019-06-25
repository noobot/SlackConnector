using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SlackConnector.Connections;

namespace SlackConnector.Connections.Clients.Conversation
{
    public interface IConversationClient
    {
		Task<Models.ConversationChannel> Create(string slackKey, string name, bool isPrivate);

		Task Close(string slackKey, string name);

		Task<Models.ConversationChannel> Info(string slackKey, string channel, bool? includeLocale = null);

		Task<Models.ConversationChannel> Invite(string slackKey, string channel, params string[] users);

		Task<Models.ConversationChannel> Join(string slackKey, string channel);

		Task Leave(string slackKey, string channel);

		Task<CursoredResponse<Models.ConversationChannel>> List(string slackKey, string cursor = null, bool? excludeArchived = null, int? limit = null, string[] types = null);

		Task<CursoredResponse<string>> Members(string slackKey, string channel, string cursor = null, int? limit = null);

		Task<Models.ConversationChannel> Open(string slackKey, string channel = null, bool? returnIm = null, params string[] users);

		Task<CursoredResponse<Models.ConversationMessage>> Replies(string slackKey, string channel, string timestamp, string cursor = null, bool? inclusive = null, string latest = null, int? limit = null, string oldest = null);
	}
}
