using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Auth
{
	public class FlurlAuthClient : IAuthClient
	{
		private readonly IResponseVerifier responseVerifier;
		public const string AUTH_TEST_PATH = "/api/auth.test";
		public const string OAUTH_ACCESS = "/api/oauth.access";

		public FlurlAuthClient(IResponseVerifier responseVerifier)
		{
			this.responseVerifier = responseVerifier;
		}

		public async Task<OAuthAccessResponse> OAuthAccess(string clientId, string clientSecret, string code, string redirectUri, string state)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(OAUTH_ACCESS)
					   .PostUrlEncodedAsync(new {
						   client_id = clientId,
						   client_secret = clientSecret,
						   code,
						   redirect_uri = redirectUri,
						   state
					   })
					   .ReceiveJson<OAuthAccessResponse>();

			responseVerifier.VerifyResponse(response);
			return response;
		}

		public async Task<AuthTest> Test(string slackKey)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(AUTH_TEST_PATH)
					   .SetQueryParam("token", slackKey)
					   .GetJsonAsync<AuthTestResponse>();

			responseVerifier.VerifyResponse(response);
			return new Models.AuthTest()
			{
				Team = response.Team,
				TeamId = response.TeamId,
				Url = response.Url,
				User = response.User,
				UserId = response.UserId,
			};
		}
	}
}
