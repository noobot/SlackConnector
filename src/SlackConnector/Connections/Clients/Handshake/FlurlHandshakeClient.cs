using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Handshake
{
    internal class FlurlHandshakeClient : IHandshakeClient
    {
        internal const string HANDSHAKE_PATH = "/api/rtm.start";

        public async Task<HandshakeResponse> FirmShake(string slackKey)
        {
            return await ClientConstants
                       .HANDSHAKE_PATH
                       .AppendPathSegment(HANDSHAKE_PATH)
                       .SetQueryParam("token", slackKey)
                       .GetJsonAsync<HandshakeResponse>();
        }
    }
}