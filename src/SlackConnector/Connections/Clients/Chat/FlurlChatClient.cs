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

        public FlurlChatClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments)
        {
            var response = await ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(SEND_MESSAGE_PATH)
                       .WithOAuthBearerToken(slackKey)
                       .PostJsonAsync(new {
                           channel = channel,
                           text = text,
                           as_user = true,
                           link_names = true,
                           attachments = attachments
                       }).ReceiveJson<StandardResponse>();

            _responseVerifier.VerifyResponse(response);
        }
    }
}