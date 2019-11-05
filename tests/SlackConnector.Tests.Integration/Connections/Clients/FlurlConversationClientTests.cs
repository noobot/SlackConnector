using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Conversation;
using SlackConnector.Tests.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackConnector.Tests.Integration.Connections.Clients
{
	public class FlurlConversationClientTests
	{
		[Fact]
		public async Task should_list_channels_with_flurl()
		{
			var config = new ConfigReader().GetConfig();
			var client = new FlurlConversationClient(new ResponseVerifier());

			// when
			var response = await client.List(config.Slack.ApiToken);

			// then
			Assert.NotEmpty(response.Items);
		}
	}
}
