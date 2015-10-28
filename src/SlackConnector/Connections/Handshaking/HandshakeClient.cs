using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Handshaking.Models;

namespace SlackConnector.Connections.Handshaking
{
    internal class HandshakeClient : IHandshakeClient
    {
        internal const string HANDSHAKE_PATH = "/api/rtm.start";
        private readonly IRestSharpFactory _restSharpFactory;

        public HandshakeClient(IRestSharpFactory restSharpFactory)
        {
            _restSharpFactory = restSharpFactory;
        }

        public async Task<SlackHandshake> FirmShake(string slackKey)
        {
            var request = new RestRequest(HANDSHAKE_PATH);
            request.AddHeader("token", slackKey);

            IRestClient client = _restSharpFactory.CreateClient("https://slack.com");
            IRestResponse response = await client.ExecutePostTaskAsync(request);

            return null;
        }
    }
}