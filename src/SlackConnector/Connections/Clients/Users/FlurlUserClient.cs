using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Users
{
	public class FlurlUserClient : IUserClient
	{
		private readonly IResponseVerifier responseVerifier;
		public const string USERS_LIST_PATH = "/api/users.list";
		public const string USERS_INFO_PATH = "/api/users.info";
		public const string USERS_IDENTITY_PATH = "/api/users.identity";

		public FlurlUserClient(IResponseVerifier responseVerifier)
		{
			this.responseVerifier = responseVerifier;
		}

		public async Task<User> Info(string slackKey, string userId, bool? includeLocale = null)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(USERS_INFO_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("user", userId)
					   .SetQueryParam("include_locale", includeLocale)
					   .GetJsonAsync<UserResponse>();

			responseVerifier.VerifyResponse(response);
			return response.User;
		}

		public async Task<CursoredResponse<User>> List(string slackKey, string cursor = null, int? limit = null)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(USERS_LIST_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("cursor", cursor)
					   .SetQueryParam("limit", limit)
					   .GetJsonAsync<UserCollectionResponse>();

			responseVerifier.VerifyResponse(response);
			return new CursoredResponse<User>(response.Members, response.ReponseMetadata?.NextCursor);
		}

		public async Task<ICollection<User>> ListAll(string slackKey)
		{
			string cursor = null;
			var userList = new List<User>();
			do
			{
				var response = await this.List(slackKey, cursor, 200);
				userList.AddRange(response);
			} while (cursor != null);
			return userList;
		}

		public async Task<Identity> Identity(string slackKey)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(USERS_IDENTITY_PATH)
					   .SetQueryParam("token", slackKey)
					   .GetJsonAsync<IdentityResponse>();

			responseVerifier.VerifyResponse(response);
			return new Identity(response.User, response.Team);
		}
	}
}
