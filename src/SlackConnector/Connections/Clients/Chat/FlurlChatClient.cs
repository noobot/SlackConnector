using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal class FlurlChatClient : IChatClient
    {
        private readonly IResponseVerifier _responseVerifier;
        internal const string SEND_MESSAGE_PATH = "/api/chat.postMessage";
        internal const string DELETE_MESSAGE_PATH = "/api/chat.delete";

        public FlurlChatClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task<PostMessageResponse> PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var request = ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(SEND_MESSAGE_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channel", channel)
                       .SetQueryParam("text", text)
                       .SetQueryParam("as_user", "true");

            if (attachments != null && attachments.Any())
            {
                request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
            }

            var response = await request.GetJsonAsync<PostMessageResponse>();
            _responseVerifier.VerifyResponse(response);
            return response;
        }

        public async Task<DeleteMessageResponse> DeleteMessage(string slackKey, string channel, string timeStamp)
        {
            var request = ClientConstants
                .SlackApiHost
                .AppendPathSegment(DELETE_MESSAGE_PATH)
                .SetQueryParam("token", slackKey)
                .SetQueryParam("channel", channel)
                .SetQueryParam("ts", timeStamp)
                .SetQueryParam("as_user", "true");

            var response = await request.GetJsonAsync<DeleteMessageResponse>();
            _responseVerifier.VerifyResponse(response);
            return response;
        }
    }
}