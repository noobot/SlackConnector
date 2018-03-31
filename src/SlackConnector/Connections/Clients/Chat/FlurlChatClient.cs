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
    public class FlurlChatClient : IChatClient
    {
        private readonly IResponseVerifier _responseVerifier;
        internal const string SEND_MESSAGE_PATH = "/api/chat.postMessage";
		internal const string UPDATE_MESSAGE_PATH = "/api/chat.update";

		public FlurlChatClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task PostMessage(string slackKey, string channel, string text, 
			IList<SlackAttachment> attachments, string threadTs = null, string iconUrl = null, 
			string userName = null, bool asUser = false, bool linkNames = true)
        {
            var request = ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(SEND_MESSAGE_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channel", channel)
                       .SetQueryParam("text", text)
                       .SetQueryParam("as_user", asUser)
                       .SetQueryParam("link_names", linkNames)
					   .SetQueryParam("thread_ts", threadTs)
            		   .SetQueryParam("icon_url", iconUrl)
					   .SetQueryParam("username", userName);

            if (attachments != null && attachments.Any())
            {
                request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
            }

			var response = await request.GetJsonAsync<StandardResponse>();
            _responseVerifier.VerifyResponse(response);
        }

		public async Task Update(string slackKey, string messageTs, string channel, string text, IList<SlackAttachment> attachments, bool asUser = false, bool linkNames = true)
		{
			var request = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(UPDATE_MESSAGE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", channel)
					   .SetQueryParam("text", text)
					   .SetQueryParam("as_user", asUser)
					   .SetQueryParam("link_names", linkNames)
					   .SetQueryParam("ts", messageTs);

			if (attachments != null && attachments.Any())
			{
				request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
			}

			var response = await request.GetJsonAsync<StandardResponse>();
			_responseVerifier.VerifyResponse(response);
		}
	}
}