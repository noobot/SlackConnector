using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Conversation
{
	public class FlurlConversationClient : IConversationClient
	{
		internal const string CONVERSATION_CLOSE_PATH = "/api/conversation.close";
		internal const string CONVERSATION_CREATE_PATH = "/api/conversation.create";
		internal const string CONVERSATION_INFO_PATH = "/api/conversation.info";
		internal const string CONVERSATION_INVITE_PATH = "/api/conversation.invite";
		internal const string CONVERSATION_JOIN_PATH = "/api/conversation.join";
		internal const string CONVERSATION_LEAVE_PATH = "/api/conversation.leave";
		internal const string CONVERSATION_LIST_PATH = "/api/conversation.list";
		internal const string CONVERSATION_MEMBERS_PATH = "/api/conversation.members";
		internal const string CONVERSATION_OPEN_PATH = "/api/conversation.open";
		internal const string CONVERSATION_REPLIES_PATH = "/api/conversation.replies";
		private readonly IResponseVerifier responseVerifier;

		public FlurlConversationClient(IResponseVerifier responseVerifier)
		{
			this.responseVerifier = responseVerifier;
		}

		public async Task Close(string slackKey, string name)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_CLOSE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .GetJsonAsync<StandardResponse>();

			responseVerifier.VerifyResponse(response);
		}

		public async Task<Models.ConversationChannel> Create(string slackKey, string name, bool isPrivate)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_CREATE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .SetQueryParam("is_private", isPrivate)
					   .GetJsonAsync<ConversationResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Channel;
		}

		public async Task<Models.ConversationChannel> Info(string slackKey, string name, bool? includeLocale = null)
		{
			var url = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_INFO_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name);
			if (includeLocale.HasValue)
				url.SetQueryParam("include_locale", includeLocale.Value);

			var response = await url
					   .GetJsonAsync<ConversationResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Channel;
		}

		public async Task<Models.ConversationChannel> Invite(string slackKey, string name, params string[] users)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_INVITE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .SetQueryParam("users", string.Join(",", users))
					   .GetJsonAsync<ConversationResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Channel;
		}

		public async Task<Models.ConversationChannel> Join(string slackKey, string name)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_JOIN_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .GetJsonAsync<ConversationResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Channel;
		}

		public async Task Leave(string slackKey, string name)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_LEAVE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .GetJsonAsync<StandardResponse>();

			responseVerifier.VerifyResponse(response);
		}

		public async Task<CursoredResponse<Models.ConversationChannel>> List(string slackKey, string cursor = null, bool? excludeArchived = null, int? limit = null, string[] types = null)
		{
			var url = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_JOIN_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("cursor", cursor)
						.SetQueryParam("excluded_archived", excludeArchived)
						.SetQueryParam("limit", limit.Value);

			if (types != null)
				url.SetQueryParam("types", string.Join(",", types));


			var response = await url
					   .GetJsonAsync<ConversationCollectionReponse>();

			responseVerifier.VerifyResponse(response);
			return new CursoredResponse<Models.ConversationChannel>(response.Channels, response.ReponseMetadata?.NextCursor);
		}

		public async Task<CursoredResponse<string>> Members(string slackKey, string name, string cursor = null, int? limit = null)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_JOIN_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .SetQueryParam("cursor", cursor)
					   .SetQueryParam("limit", limit)
					   .GetJsonAsync<ConversationMembersResponse>();

			responseVerifier.VerifyResponse(response);
			return new CursoredResponse<string>(response.Members, response.ReponseMetadata?.NextCursor);
		}

		public async Task<Models.ConversationChannel> Open(string slackKey, string name = null, bool? returnIm = null, params string[] users)
		{
			var url = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_JOIN_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .SetQueryParam("return_im", returnIm);

			if (users != null)
				url.SetQueryParam("users", string.Join(",", users));

			var response = await url
					   .GetJsonAsync<ConversationResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Channel;
		}

		public async Task<CursoredResponse<ConversationMessage>> Replies(string slackKey, string name, string timestamp, string cursor = null, bool? inclusive = null, string latest = null, int? limit = null, string oldest = null)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(CONVERSATION_JOIN_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", name)
					   .SetQueryParam("ts", timestamp)
					   .SetQueryParam("cursor", cursor)
					   .SetQueryParam("inclusive", inclusive)
					   .SetQueryParam("latest", latest)
					   .SetQueryParam("limit", limit)
					   .SetQueryParam("oldest", oldest)
					   .GetJsonAsync<ConversationMessageReponse>();

			responseVerifier.VerifyResponse(response);
			return new CursoredResponse<ConversationMessage>(response.Messages, response.ReponseMetadata?.NextCursor);
		}
	}
}
