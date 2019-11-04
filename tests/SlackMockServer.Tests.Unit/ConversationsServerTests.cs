using AutoFixture.Xunit2;
using DeepEqual.Syntax;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackMockServer.Tests.Unit
{
	public class ConversationsServerTests
	{
		private int GetRandomPort => (new Random()).Next(3000, 4000);

		[Theory, AutoData]
		public async Task WhenAskingConversationListThenServerReceive(SlackConnector.Connections.Models.ConversationChannel[] channels)
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockConversationList(conversations: channels);

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlConversationClient(new ResponseVerifier());

				var response = await client.List("SLACK_KEY");

				var responseChannels = response.Items.ToList();
				Assert.Equal(responseChannels.Count, channels.Length);
				for (int i = 0; i < channels.Length; i++)
				{
					Assert.Equal(responseChannels[i].Id, channels[i].Id);
				}
			}
		}
	}
}
