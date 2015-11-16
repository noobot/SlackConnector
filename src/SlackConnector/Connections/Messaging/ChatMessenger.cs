using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;
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

        public async Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var client = _restSharpFactory.CreateClient("https://slack.com");

            var request = new RestRequest(SEND_MESSAGE_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channel", channel);
            request.AddParameter("text", text);
            request.AddParameter("as_user", "true");

            if (attachments != null && attachments.Any())
            {
                request.AddParameter("attachment", JsonConvert.SerializeObject(attachments));
            }

            IRestResponse response = await client.ExecutePostTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CommunicationException($"Error occured while posting message '{response.StatusCode}'");
            }

            StandardResponse slackResponse = JsonConvert.DeserializeObject<StandardResponse>(response.Content);
            if (!slackResponse.Ok)
            {
                throw new CommunicationException($"Error occured while posting message '{slackResponse.Error}'");
            }
        }
    }
}