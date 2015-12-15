using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Handshake
{
    internal class HandshakeClient : IHandshakeClient
    {
        private readonly IRequestExecutor _requestExecutor;
        internal const string HANDSHAKE_PATH = "/api/rtm.start";

        public HandshakeClient(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        public async Task<HandshakeResponse> FirmShake(string slackKey)
        {
            var request = new RestRequest(HANDSHAKE_PATH);
            request.AddParameter("token", slackKey);

            return await _requestExecutor.Execute<HandshakeResponse>(request);
        }
    }
}