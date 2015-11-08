using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SlackConnector.Connections.Models;
using SlackConnector.Exceptions;

namespace SlackConnector.Connections.Messaging
{
    internal class ChannelMessenger : IChannelMessenger
    {
        internal const string JOIN_DM_PATH = "/api/im.open";
        private readonly IRestSharpFactory _restSharpFactory;

        public ChannelMessenger(IRestSharpFactory restSharpFactory)
        {
            _restSharpFactory = restSharpFactory;
        }

        public async Task<Channel> JoinDirectMessageChannel(string slackKey, string user)
        {
            var client = _restSharpFactory.CreateClient("https://slack.com");

            var request = new RestRequest(JOIN_DM_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("user", user);

            IRestResponse response = await client.ExecutePostTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CommunicationException($"Error occured while joining channel '{response.StatusCode}'");
            }

            JoinChannelResponse slackResponse = JsonConvert.DeserializeObject<JoinChannelResponse>(response.Content);
            if (!slackResponse.Ok)
            {
                throw new CommunicationException($"Error occured while joining channel '{slackResponse.Error}'");
            }

            return slackResponse.Channel;
        }
    }
}