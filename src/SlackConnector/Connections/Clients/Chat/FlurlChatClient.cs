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
        internal const string POST_MESSAGE_PATH = "/api/chat.postMessage";
        internal const string UPDATE_MESSAGE_PATH = "/api/chat.update";

        public FlurlChatClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task<string> PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var request = ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(POST_MESSAGE_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channel", channel)
                       .SetQueryParam("text", text)
                       .SetQueryParam("as_user", "true");
            
            if (attachments != null && attachments.Any())
            {
                request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
            }

            var response = await request.GetJsonAsync<StandardResponse>();
            _responseVerifier.VerifyResponse(response);
            return response.Ts;
        }


        public async Task<string> UpdateMessage(string slackKey, string timeStamp, string channel, string text, IList<SlackAttachment> attachments)
        {
            var request = ClientConstants
                .SlackApiHost
                .AppendPathSegment(UPDATE_MESSAGE_PATH)
                .SetQueryParam("token", slackKey)
                .SetQueryParam("ts", timeStamp)
                .SetQueryParam("channel", channel)
                .SetQueryParam("text", text)
                .SetQueryParam("as_user", "true");

            if (attachments != null && attachments.Any())
            {
                request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
            }

            var response = await request.GetJsonAsync<StandardResponse>();
            _responseVerifier.VerifyResponse(response);
            return response.Ts;
        }
    }
}