using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Channel
{
    internal class ChannelClient : IChannelClient
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        private readonly IRestSharpFactory _restSharpFactory;
        private readonly IResponseVerifier _responseVerifier;

        public ChannelClient(IRestSharpFactory restSharpFactory, IResponseVerifier responseVerifier)
        {
            _restSharpFactory = restSharpFactory;
            _responseVerifier = responseVerifier;
        }

        public async Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user)
        {
            var client = _restSharpFactory.CreateClient("https://slack.com");

            var request = new RestRequest(JOIN_DM_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("user", user);

            IRestResponse response = await client.ExecutePostTaskAsync(request);
            return _responseVerifier.VerifyResponse<JoinChannelResponse>(response).Channel;
        }
    }
}