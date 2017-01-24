using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Clients.Channel
{
    internal class FlurlChannelClient : IChannelClient
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        internal const string CHANNELS_LIST_PATH = "/api/channels.list";
        internal const string GROUPS_LIST_PATH = "/api/groups.list";
        internal const string USERS_LIST_PATH = "/api/users.list";

        public Task<Models.Channel> JoinDirectMessageChannel(string slackKey, string user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Models.Channel[]> GetChannels(string slackKey)
        {
            return await ClientConstants
                       .HANDSHAKE_PATH
                       .AppendPathSegment(CHANNELS_LIST_PATH)
                       .SetQueryParam("token", slackKey)
                       .GetJsonAsync<Models.Channel[]>();
        }

        public async Task<Group[]> GetGroups(string slackKey)
        {
            return await ClientConstants
                       .HANDSHAKE_PATH
                       .AppendPathSegment(GROUPS_LIST_PATH)
                       .SetQueryParam("token", slackKey)
                       .GetJsonAsync<Group[]>();
        }

        public async Task<User[]> GetUsers(string slackKey)
        {
            return await ClientConstants
                       .HANDSHAKE_PATH
                       .AppendPathSegment(USERS_LIST_PATH)
                       .SetQueryParam("token", slackKey)
                       .GetJsonAsync<User[]>();
        }
    }
}