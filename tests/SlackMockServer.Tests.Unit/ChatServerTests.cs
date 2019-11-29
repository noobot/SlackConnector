using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Chat;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SlackMockServer.Tests.Unit
{
	public class ChatServerTests
	{
		private int GetRandomPort => (new Random()).Next(3000, 4000);

		[Fact]
		public async Task WhenSendingMessageThenServerReceive()
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockDefaultSendMessage();

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlChatClient(new ResponseVerifier());

				var channel = "fake-channel";
				var text = "This is a message for a test";
				var response = await client.PostMessage("fake-key", channel, text);

				Assert.Equal(channel, response.Channel);
				Assert.Equal(text, response.Message.Text);
			}
		}

		[Fact]
		public async Task WhenDeletingMessageThenServerReceive()
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockDefaultDeleteMessage();

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlChatClient(new ResponseVerifier());

				var channel = "fake-channel";
				var ts = "1111.222223";
				await client.Delete("fake-key", channel, ts);

				var logEntry = server.HttpServer.LogEntries.Filter(FlurlChatClient.DELETE_MESSAGE_PATH, _ => true);

				Assert.NotNull(logEntry);
			}
		}

		[Fact]
		public async Task WhenUpdatingMessageThenServerReceive()
		{
			var port = GetRandomPort;
			using (var server = new SlackServer(port))
			{
				server.MockDefaultUpdateMessage();

				ClientConstants.SlackApiHost = $"http://localhost:{port}";

				var client = new FlurlChatClient(new ResponseVerifier());

				var channel = "fake-channel";
				var ts = "1111.222223";
				var text = "This is a message for a test update";
				await client.Update("fake-key", ts, channel, text);

				var logEntry = server.HttpServer.LogEntries.Filter(FlurlChatClient.UPDATE_MESSAGE_PATH, _ => true);

				Assert.NotNull(logEntry);
			}
		}
	}
}
