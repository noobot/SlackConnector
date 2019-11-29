using Shouldly;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Team;
using SlackLibrary.Tests.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackLibrary.Tests.Integration.Connections.Clients
{
	public class FlurlTeamClientTests
	{
		[Fact]
		public async Task should_call_team_info_with_flurl()
		{
			var config = new ConfigReader().GetConfig();
			var client = new FlurlTeamClient(new ResponseVerifier());

			// when
			var response = await client.GetTeamInfo(config.Slack.ApiToken);

			// then
			response.ShouldNotBeNull();
		}
	}
}
