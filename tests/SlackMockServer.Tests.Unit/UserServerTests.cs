using AutoFixture.Xunit2;
using DeepEqual.Syntax;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackMockServer.Tests.Unit
{
	public class UserServerTests
	{
		private int GetRandomPort => (new Random()).Next(3000, 4000);

		[Theory, AutoData]
		public async Task WhenAskingForUserInfoThenServerReceive(SlackLibrary.Connections.Models.User wantedResponse)
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockUserInfo(wantedResponse);

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlUserClient(new ResponseVerifier());

				var response = await client.Info("SLACK_KEY", wantedResponse.Id);

				response.WithDeepEqual(wantedResponse)
					.IgnoreSourceProperty(x => x.Updated)
					.Assert();
			}
		}
	}
}
