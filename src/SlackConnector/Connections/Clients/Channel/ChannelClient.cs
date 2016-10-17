using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Channel
{
    internal class ChannelClient : IChannelClient
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        internal const string CHANNELS_LIST_PATH = "/api/channels.list";
        internal const string GROUPS_LIST_PATH = "/api/groups.list";
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

        public async Task<Models.Channel[]> GetChannels(string slackKey)
        {
            var request = new RestRequest(CHANNELS_LIST_PATH);
            request.AddParameter("token", slackKey);

            var response = await _requestExecutor.Execute<ChannelsResponse>(request);
            return response.Channels;
        }

        public async Task<Models.Group[]> GetGroups(string slackKey)
        {
            var request = new RestRequest(GROUPS_LIST_PATH);
            request.AddParameter("token", slackKey);

            var response = await _requestExecutor.Execute<GroupsResponse>(request);
            return response.Groups;
        }
    }
}