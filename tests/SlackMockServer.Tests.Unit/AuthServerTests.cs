using AutoFixture.Xunit2;
using DeepEqual.Syntax;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackMockServer.Tests.Unit
{
	public class AuthServerTests
	{
		private int GetRandomPort => (new Random()).Next(3000, 4000);

		[Theory, AutoData]
		public async Task WhenCallingOAuthAccessThenServerReceive(SlackLibrary.Connections.Responses.OAuthAccessResponse wantedResponse)
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockDefaultOAuthAccess(wantedResponse);

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlAuthClient(new ResponseVerifier());

				var response = await client.OAuthAccess("CLIENT_ID", "CLIENT_SECRET", "CODE", "REDIRECT_URI", "STATE");

				response.ShouldDeepEqual(wantedResponse);
			}
		}

		[Theory, AutoData]
		public async Task WhenCallingAuthTestThenServerReceive(SlackLibrary.Connections.Models.AuthTest wantedResponse)
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				var mockedResponse = new SlackLibrary.Connections.Responses.AuthTestResponse()
				{
					Ok = true,
					TeamId = wantedResponse.TeamId,
					Team = wantedResponse.Team,
					Url = wantedResponse.Url,
					User = wantedResponse.User,
					UserId = wantedResponse.UserId
				};
				server.MockDefaultAuthTest(mockedResponse);

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlAuthClient(new ResponseVerifier());

				var response = await client.Test("SLACK_KEY");

				response.ShouldDeepEqual(wantedResponse);
			}
		}
	}
}
