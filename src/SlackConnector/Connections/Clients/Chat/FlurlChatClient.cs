using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;
using SlackConnector.Models.Blocks;

namespace SlackConnector.Connections.Clients.Chat
{
    public class FlurlChatClient : IChatClient
    {
        private readonly IResponseVerifier _responseVerifier;
        public const string SEND_MESSAGE_PATH = "/api/chat.postMessage";
		public const string SEND_EPHEMERAL_PATH = "/api/chat.postEphemeral";
		public const string UPDATE_MESSAGE_PATH = "/api/chat.update";
		public const string DELETE_MESSAGE_PATH = "/api/chat.delete";

		public FlurlChatClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

		public async Task<MessageResponse> PostMessage(string slackKey, string channel, string text,
			IEnumerable<SlackAttachment> attachments = null, string threadTs = null, string iconUrl = null,
			string userName = null, bool asUser = false, bool linkNames = true, IEnumerable<BlockBase> blocks = null)
		{
			var request = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(SEND_MESSAGE_PATH);

			var args = new Dictionary<string, string>()
			{
				{ "token", slackKey },
				{"channel", channel },
				{"text", text },
				{"as_user", asUser.ToString().ToLower() },
				{"link_names", linkNames.ToString().ToLower() },
				{"thread_ts", threadTs },
				{"icon_url", iconUrl },
				{"username", userName }
			};

            if (attachments != null && attachments.Any())
            {
				args.Add("attachments", JsonConvert.SerializeObject(attachments));
            }

			if (blocks != null && blocks.Any())
			{
				var jsonBlocks = JsonConvert.SerializeObject(blocks);
				args.Add("blocks", jsonBlocks);
			}

			var response = await request.PostUrlEncodedAsync(args)
				.ReceiveJson<MessageResponse>();

            _responseVerifier.VerifyResponse(response);
			return response;
        }

		public async Task Update(string slackKey, string messageTs, string channel, string text, IEnumerable<SlackAttachment> attachments = null, 
			bool asUser = false, bool linkNames = true, IEnumerable<BlockBase> blocks = null)
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

			if (blocks != null && blocks.Any())
			{
				request.SetQueryParam("blocks", JsonConvert.SerializeObject(blocks));
			}

			var response = await request.GetJsonAsync<DefaultStandardResponse>();
			_responseVerifier.VerifyResponse(response);
		}

		public async Task Update(string responseUrl, string text, IEnumerable<SlackAttachment> attachments = null, IEnumerable<BlockBase> blocks = null)
		{
			var response = await responseUrl.PostJsonAsync(new
			{
				text = text,
				attachments = attachments,
				blocks = blocks
			}).ReceiveJson<DefaultStandardResponse>();
			_responseVerifier.VerifyResponse(response);
		}

		public async Task Delete(string slackKey, string channel, string ts, bool asUser = true)
		{
			var request = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(DELETE_MESSAGE_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", channel)
					   .SetQueryParam("ts", ts)
					   .SetQueryParam("as_user", asUser);


			var response = await request.GetJsonAsync<DefaultStandardResponse>();
			_responseVerifier.VerifyResponse(response);
		}

		public async Task<MessageResponse> PostEphemeral(string slackKey, string channel, string user, string text,
			IEnumerable<SlackAttachment> attachments = null, string threadTs = null, string iconUrl = null, string userName = null, 
			bool asUser = false, bool linkNames = true, IEnumerable<BlockBase> blocks = null)
		{
			var request = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(SEND_EPHEMERAL_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("channel", channel)
					   .SetQueryParam("text", text)
					   .SetQueryParam("user", user)
					   .SetQueryParam("as_user", asUser)
					   .SetQueryParam("link_names", linkNames)
					   .SetQueryParam("thread_ts", threadTs)
					   .SetQueryParam("icon_url", iconUrl)
					   .SetQueryParam("username", userName);

			if (attachments != null && attachments.Any())
			{
				request.SetQueryParam("attachments", JsonConvert.SerializeObject(attachments));
			}

			if (blocks != null && blocks.Any())
			{
				request.SetQueryParam("blocks", JsonConvert.SerializeObject(blocks));
			}

			var response = await request.GetJsonAsync<MessageResponse>();
			_responseVerifier.VerifyResponse(response);
			return response;
		}
	}
}