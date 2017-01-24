using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Channel
{
    internal class RestSharpChannelClient : IChannelClient
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        internal const string CHANNELS_LIST_PATH = "/api/channels.list";
        internal const string GROUPS_LIST_PATH = "/api/groups.list";
        internal const string USERS_LIST_PATH = "/api/users.list";
        private readonly IRestSharpRequestExecutor _restSharpRequestExecutor;

        public RestSharpChannelClient(IRestSharpRequestExecutor restSharpRequestExecutor)
        {
            _restSharpRequestExecutor = restSharpRequestExecutor;
        }

        public async Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user)
        {
            var request = new RestRequest(JOIN_DM_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("user", user);

            var response = await _restSharpRequestExecutor.Execute<JoinChannelResponse>(request);
            return response.Channel;
        }

        public async Task<Models.Channel[]> GetChannels(string slackKey)
        {
            var request = new RestRequest(CHANNELS_LIST_PATH);
            request.AddParameter("token", slackKey);

            var response = await _restSharpRequestExecutor.Execute<ChannelsResponse>(request);
            return response.Channels;
        }

        public async Task<Models.Group[]> GetGroups(string slackKey)
        {
            var request = new RestRequest(GROUPS_LIST_PATH);
            request.AddParameter("token", slackKey);

            var response = await _restSharpRequestExecutor.Execute<GroupsResponse>(request);
            return response.Groups;
        }

        public async Task<User[]> GetUsers(string slackKey)
        {
            var request = new RestRequest(USERS_LIST_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("presence", "1");

            var response = await _restSharpRequestExecutor.Execute<UsersResponse>(request);
            return response.Members;
        }
    }
}