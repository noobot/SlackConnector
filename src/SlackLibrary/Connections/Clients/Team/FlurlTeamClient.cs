using Flurl;
using Flurl.Http;
using SlackLibrary.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Team
{
    public class FlurlTeamClient : ITeamClient
    {
		public const string TEAM_INFO = "/api/team.info";
		private readonly IResponseVerifier responseVerifier;

		public FlurlTeamClient(IResponseVerifier responseVerifier)
		{
			this.responseVerifier = responseVerifier;
		}

		public async Task<Models.Team> GetTeamInfo(string slackKey)
		{
			var response = await ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(TEAM_INFO)
					   .SetQueryParam("token", slackKey)
					   .GetJsonAsync<TeamInfoResponse>();

			responseVerifier.VerifyResponse(response);
			return response.Team;
		}
	}
}
