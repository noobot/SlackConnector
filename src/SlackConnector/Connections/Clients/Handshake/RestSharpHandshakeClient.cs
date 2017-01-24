using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Handshake
{
    internal class RestSharpHandshakeClient : IHandshakeClient
    {
        private readonly IRestSharpRequestExecutor _restSharpRequestExecutor;
        internal const string HANDSHAKE_PATH = "/api/rtm.start";

        public RestSharpHandshakeClient(IRestSharpRequestExecutor restSharpRequestExecutor)
        {
            _restSharpRequestExecutor = restSharpRequestExecutor;
        }

        public async Task<HandshakeResponse> FirmShake(string slackKey)
        {
            var request = new RestRequest(HANDSHAKE_PATH);
            request.AddParameter("token", slackKey);

            return await _restSharpRequestExecutor.Execute<HandshakeResponse>(request);
        }
    }
}