using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Channel
{
    internal class ChannelClient : IChannelClient
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        private readonly IRequestExecutor _requestExecutor;

        public ChannelClient(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        public async Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user)
        {
            var request = new RestRequest(JOIN_DM_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("user", user);

            var response = await _requestExecutor.Execute<JoinChannelResponse>(request);
            return response.Channel;
        }
    }
}