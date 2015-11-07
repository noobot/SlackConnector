using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Models;

namespace SlackConnector.Connections.Messaging
{
    internal class ChatMessenger : IChatMessenger
    {
        internal const string SEND_MESSAGE_PATH = "/api/chat.postMessage";
        private readonly IRestSharpFactory _restSharpFactory;

        public ChatMessenger(IRestSharpFactory restSharpFactory)
        {
            _restSharpFactory = restSharpFactory;
        }

        public async Task PostMessage(string slackKey, string channel, string text, SlackAttachment[] attachments)
        {
            var client = _restSharpFactory.CreateClient("https://slack.com");

            var request = new RestRequest(SEND_MESSAGE_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channel", channel);
            request.AddParameter("text", text);
            request.AddParameter("as_user", "true");

            await client.ExecutePostTaskAsync(request);
        }
    }
}