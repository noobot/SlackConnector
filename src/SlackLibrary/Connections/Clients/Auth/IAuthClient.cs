using SlackLibrary.Connections.Models;
using SlackLibrary.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Auth
{
	public interface IAuthClient
	{
		Task<AuthTest> Test(string slackKey);

		Task<OAuthAccessResponse> OAuthAccess(string clientId, string clientSecret, string code, string redirectUri, string state);
	}
}
