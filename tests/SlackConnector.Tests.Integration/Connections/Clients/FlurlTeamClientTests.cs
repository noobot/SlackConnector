using Shouldly;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Team;
using SlackConnector.Tests.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackConnector.Tests.Integration.Connections.Clients
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
