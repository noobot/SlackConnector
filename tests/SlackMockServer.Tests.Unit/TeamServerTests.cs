using AutoFixture.Xunit2;
using DeepEqual.Syntax;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Team;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackMockServer.Tests.Unit
{
	public class TeamServerTests
	{
		private int GetRandomPort => (new Random()).Next(3000, 4000);

		[Theory, AutoData]
		public async Task WhenAskingTeamInfoThenServerReceive(SlackConnector.Connections.Models.Team wantedResponse)
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockDefaultTeamInfo(wantedResponse);

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlTeamClient(new ResponseVerifier());

				var response = await client.GetTeamInfo("SLACK_KEY");

				response.ShouldDeepEqual(wantedResponse);
			}
		}
	}
}
