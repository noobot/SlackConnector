using Shouldly;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Auth;
using SlackConnector.Tests.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackConnector.Tests.Integration.Connections.Clients
{
	public class FlurlAuthClientTests
	{
		[Fact]
		public async Task should_call_auth_test_with_flurl()
		{
			var config = new ConfigReader().GetConfig();
			var client = new FlurlAuthClient(new ResponseVerifier());

			// when
			var response = await client.Test(config.Slack.ApiToken);

			// then
			response.ShouldNotBeNull();
		}
	}
}
