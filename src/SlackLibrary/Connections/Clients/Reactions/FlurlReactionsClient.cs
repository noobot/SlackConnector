using Flurl;
using Flurl.Http;
using SlackLibrary.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Reactions
{
	public class FlurlReactionsClient : IReactionsClient
	{
		public const string REACTIONS_ADD_PATH = "/api/reactions.add";
		public const string REACTIONS_REMOVE_PATH = "/api/reactions.remove";

		private readonly IResponseVerifier responseVerifier;

		public FlurlReactionsClient(IResponseVerifier responseVerifier)
		{
			this.responseVerifier = responseVerifier;
		}

		public async Task Add(string slackKey, string name, string channel = null, string messageTs = null, string file = null, string fileComment = null)
		{
			var request = ClientConstants
					   .SlackApiHost
					   .AppendPathSegment(REACTIONS_ADD_PATH)
					   .SetQueryParam("token", slackKey)
					   .SetQueryParam("name", name)
					   .SetQueryParam("channel", channel)
					   .SetQueryParam("timestamp", messageTs)
					   .SetQueryParam("file", file)
					   .SetQueryParam("file_comment", fileComment);

			var response = await request.GetJsonAsync<DefaultStandardResponse>();
			if (!response.Ok)
			{
				switch (response.Error)
				{
					case "already_reacted":
					case "too_many_emoji":
					case "too_many_reactions":
						return;
				}
			}
			responseVerifier.VerifyResponse(response);
		}

		public async Task Remove(string slackKey, string name, string channel = null, string messageTs = null, string file = null, string fileComment = null)
		{
			var request = ClientConstants
		   .SlackApiHost
		   .AppendPathSegment(REACTIONS_REMOVE_PATH)
		   .SetQueryParam("token", slackKey)
		   .SetQueryParam("name", name)
		   .SetQueryParam("channel", channel)
		   .SetQueryParam("timestamp", messageTs)
		   .SetQueryParam("file", file)
		   .SetQueryParam("file_comment", fileComment);

			var response = await request.GetJsonAsync<DefaultStandardResponse>();
			if (!response.Ok)
			{
				switch (response.Error)
				{
					case "already_reacted":
					case "too_many_emoji":
					case "too_many_reactions":
						return;
				}
			}
			responseVerifier.VerifyResponse(response);

		}
	}
}
