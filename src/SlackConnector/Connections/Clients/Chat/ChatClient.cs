using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal class ChatClient : IChatClient
    {
        private readonly IRequestExecutor _requestExecutor;
        internal const string SEND_MESSAGE_PATH = "/api/chat.postMessage";

        public ChatClient(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        public async Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var request = new RestRequest(SEND_MESSAGE_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channel", channel);
            request.AddParameter("text", text);
            request.AddParameter("as_user", "true");

            if (attachments != null && attachments.Any())
            {
                request.AddParameter("attachments", JsonConvert.SerializeObject(attachments));
            }

            await _requestExecutor.Execute<StandardResponse>(request);
        }
    }
}